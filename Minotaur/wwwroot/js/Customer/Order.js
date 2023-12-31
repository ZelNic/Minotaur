
let ordersData;

$(document).ready(function () {
    getOrders();
});
function getOrders() {
    let orders = document.getElementById("orders");

    $.ajax({
        url: '/Customer/Orders/Get',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            ordersData = response.data;
            orders.innerHTML = `${generateOrderCards()}`;
        },
    });
}
function generateOrderCards() {
    let html = ``;

    for (let [indexOrder, order] of ordersData.entries()) {

        let countProduct = 0;
        let sumPrice = 0;
        var tableOrder = ``;
        let functionBtnForOrder = ``;
        let showProduct;

        showProduct = order.shippedProducts == null ? order.orderedProducts : order.shippedProducts;

        if (order.orderStatus == "Завершен") {
            functionBtnForOrder = `<button onclick="reviewOrderHandler('${order.orderId}','${order.orderPickupPointId}')" class="btn btn-success"> Отзыв о заказе</button >`;
        }
        else if (order.orderStatus == "Заказ отменен, ожидается возврат средств") {
            functionBtnForOrder = ``;
        }
        else {
            functionBtnForOrder = `<button onclick="cancelOrder('${order.orderId}')" class="btn btn-success"> Отменить заказ</button>`;
        }


        for (let [indexProduct, product] of showProduct.entries()) {
            tableOrder += `
            <tbody class="table-group-divider">
                <tr>
                    <td>${product.id}</td>
                    <td id="nameProduct_${product.id}">
                        <a href="/Customer/Home/Details?id=${product.id}">${product.productName}</a>
                    </td>
                    <td>${product.price} ₽ </td>
                    <td>${product.count}</td>
                   <td>${order.orderStatus == "Завершен" ? `<button onclick="reviewProductHandler('${indexOrder}', '${indexProduct}')" class="btn btn-success">Отзыв</button>` : ``}</td>
                </tr>
            </tbody>
            `;

            countProduct += product.count;
            sumPrice += product.count * product.price;
        }

        html += `
                    <div class="mb-5">
                <div class="form-group border border-1 rounded rounded-3 p-4">
                    <div class="d-inline-flex align-items-center">
                        <span class="mr-3">Номер заказа: </span>
                        <div class="rouden rounded-1 text-bg-primary m-1">
                            ${order.orderId}
                        </div>
                    </div>

                    <div class="d-inline-flex align-items-center">
                        <span class="mr-3">Статус заказа: </span>
                        <div class="rouden rounded-1 text-bg-primary m-1">${order.orderStatus}</div>
                    </div>

                    <div class="">
                        <label>Дата и время заказа: ${order.purchaseDate} по МСК</label>
                    </div>

                    <table class="table" id="tableProduct_${order.orderId} style="display: none;">
                        <thead>
                            <tr>
                                <th scope="col">Id</th>
                                <th scope="col">Название</th>
                                <th scope="col">Цена</th>
                                <th scope="col">Количество</th>
                                <th scope="col"></th>
                            </tr>
                        </thead>
                        ${tableOrder}
                    </table>
                    <div>
                        <label>Количество товара: ${countProduct}</label>
                        <label class="m-3">Стоимость: ${sumPrice} ₽ </label>
                    </div>
                    <div class="form-group">
                        <label>Оплачено:</label>
                        <label>${order.purchaseAmount} ₽ </label>
                        ${order.refundAmount != 0 ? `<label class="bg-warning rounded rounded-3">Сумма возврата: ${order.refundAmount} ₽</label>` : ``}
                    </div>
                    <div>
                        Доставка:
                        ${order.isCourierDelivery ? "Курьером" : "В пункт выдачи"}
                        <div class="form-group">
                            ${order.isCourierDelivery ? `Адрес доставки: ${order.city}, ${order.street} ${order.houseNumber}` : ""}
                            <div class="form-group">
                                Данные получателя:
                                ${order.receiverName}
                                ${order.receiverLastName}
                                ${order.phoneNumber}
                            </div>
                        </div>
                    </div>
                    <hr/>
                    ${functionBtnForOrder}  
                </div>
            </div>
        `;
    }
    return html;
}
function cancelOrder(orderId) {
    Swal.fire({
        title: "Отменить заказ?",
        showCancelButton: true,
        confirmButtonText: "Да, отменить",
        denyButtonText: `Нет, не отменять`
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/customer/Orders/Cancel?orderId=${orderId}`,
                method: 'POST',
                success: function (response) {
                    Swal.fire("Заказ отменен. Ожидайте возврат средств");
                    getOrders();
                },
                error: function (xhr, status, error) {
                    Swal.fire("Не удалось отменить заказ");
                    getOrders();
                }
            });
        } else if (result.isDenied) {
        }
    });

}