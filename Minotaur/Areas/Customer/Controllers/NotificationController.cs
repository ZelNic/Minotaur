﻿using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Minotaur.DataAccess.Repository.IRepository;
using Minotaur.Models;
using Minotaur.Models.Models;
using Minotaur.Models.SD;
using Minotaur.Utility;

namespace Minotaur.Areas.Customer
{
    [Area("Customer")]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<MinotaurUser> _userManager;
        public NotificationController(IUnitOfWork unitOfWork, UserManager<MinotaurUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetDataNotification()
        {
            MinotaurUser? user = await _userManager.GetUserAsync(User);
            var notifications = await _unitOfWork.Notifications.GetAllAsync(u => u.RecipientId == Guid.Parse(user.Id));
            var notHiddenNotifications = notifications.OrderByDescending(n => n.SendingTime).Where(n => n.IsHidden == false)
                .Select(n => new
                {
                    n.Id,
                    n.OrderId,
                    SendingTime = n.SendingTime.ToString("dd.MM.yyyy HH:mm"),
                    n.Text,
                    n.TypeNotification,
                    n.IsHidden,
                });

            if (notHiddenNotifications == null)
            {
                return BadRequest("Уведомлений нет");
            }

            return Json(new { data = notHiddenNotifications });
        }

        [HttpPost]
        public async Task<IActionResult> HideNotification(string notificationId)
        {
            Notification? notification = await _unitOfWork.Notifications.GetAsync(n => n.Id == Guid.Parse(notificationId));
            if (notification != null)
            {
                notification.IsHidden = true;

                _unitOfWork.Notifications.Update(notification);
                _unitOfWork.Save();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> HideAllNotifications()
        {
            var user = await _userManager.GetUserAsync(User);
            var notifications = await _unitOfWork.Notifications.GetAllAsync(u => u.RecipientId == Guid.Parse(user.Id) && !u.IsHidden);

            Parallel.ForEach(notifications, notification =>
            {
                notification.IsHidden = true;
            });

            _unitOfWork.Notifications.UpdateRange(notifications.ToArray());
            _unitOfWork.Save();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendReplyIncompleteOrder(string notificationId, bool isAgree)
        {
            Notification? notification = await _unitOfWork.Notifications.GetAsync(n => n.Id == Guid.Parse(notificationId));
            if (notification == null) { return BadRequest("Уведомление не найдено"); }

            Order? order = await _unitOfWork.Orders.GetAsync(o => o.OrderId == notification.OrderId);
            if (order == null) { _unitOfWork.Notifications.Remove(notification); _unitOfWork.Save(); return BadRequest("Заказ не найден"); }

            Notification notificationForCustomer = new()
            {
                OrderId = order.OrderId,
                RecipientId = order.UserId,
                TypeNotification = NotificationSD.SimpleNotification,
                IsHidden = false,
                SendingTime = MoscowTime.GetTime(),
            };

            if (isAgree)
            {
                order.OrderStatus = StatusByOrder.BuyerAgreesNeedSend;
                notificationForCustomer.Text = "Ожидается отправка товара";
            }
            else
            {
                order.OrderStatus = StatusByOrder.BuyerDontAgreesNeedRefunded;
                notificationForCustomer.Text = "Ожидается возврат средств";

                Notification notificationForPicker = new()
                {
                    OrderId = order.OrderId,
                    SenderId = order.UserId,
                    RecipientId = order.AssemblyResponsibleWorkerId,
                    TypeNotification = NotificationSD.SimpleNotification,
                    Text = $"Покупатель отказался от заказа {order.OrderId}.",
                    SendingTime = MoscowTime.GetTime(),
                };

                _unitOfWork.Notifications.AddAsync(notificationForPicker);
            }

            notification.IsHidden = true;
            _unitOfWork.Notifications.Update(notification);
            _unitOfWork.Orders.Update(order);
            _unitOfWork.Notifications.AddAsync(notificationForCustomer);
            _unitOfWork.Save();

            return Ok();
        }
    }
}