﻿
let reviews;

$(document).ready(function () {
    getReviewsForModeration();
});

function getReviewsForModeration() {

    $.ajax({
        url: '/Operator/Reviews/GetReviewsForModeration',
        dataTypes: 'json',
        success: function (response) {
            reviews = response.data;
            generateCardReviews();
        },
        error: function (error) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'error',
                title: error.responseText
            })
        }
    });
}

function generateCardReviews() {
    let cardsReviews = ``;

    for (let [index, review] of reviews.entries()) {
        let image = ``;

        for (let img of review.photo) {
            image += `<img src="/fileStorage/productReviewFiles/${img}" alt="Не найдено" class="mb-2 mx-1 rounded rounded-1" style="width: 20%; height: 20%;">`;
        }

        cardsReviews +=
            `
      <div class="card mb-2">
        <div class="card-body">
          <p class="card-text">${review.nameUser}</p>
          <hr/>
          <button onclick="getDetails(${review.productId})" class="btn btn-warning">Товар</button>
          <h5 class="card-title">Оценка: ${review.rating} <i class="bi bi-star-fill"></i></h5>
          <p class="card-text">${review.productReviewText}</p>
          <div class="d-flex flex-wrap">
            ${image}
          </div>
        </div>
        <div class="mx-2 mt-2 mb-2">
            <button onclick="reject('${review.id}')" class="btn btn-danger bi bi-x-square">Отклонить</button>
            <button onclick="accept('${review.id}')" class="btn btn-success bi bi-check-square">Принять</button>
        </div>
      </div>
      `;
    }

    let divReviewsCard = document.getElementById("reviewsCard");
    divReviewsCard.innerHTML = cardsReviews;
}


// IDEA: Перенести уведомление в отдельный скрипт и использовать метод, сообщение отправлять в аргумент

function accept(reviewId) {
    $.ajax({
        url: `/Operator/Reviews/AcceptReview?id=${reviewId}`,
        method: 'POST',
        success: function (response) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'success',
                title: 'Отзыв одобрен'
            })
        },
        error: function (error) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 3000,
                timerProgressBar: true,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })

            Toast.fire({
                icon: 'error',
                title: error.responseText
            })
        }
    });
}
async function reject(reviewId) {
    const { value: cause } = await Swal.fire({
        input: 'textarea',
        inputLabel: 'Message',
        inputPlaceholder: 'Причина отказа',
        inputAttributes: {
            'aria-label': 'Например, нарушение авторского права'
        },
        showCancelButton: true
    })

    if (cause) {
        $.ajax({
            url: `/Operator/Reviews/RejectReview?id=${reviewId}&comment=${cause}`,
            method: 'POST',
            success: function (response) {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'success',
                    title: 'Отзыв отклонен'
                })
            },
            error: function (error) {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })

                Toast.fire({
                    icon: 'error',
                    title: error.responseText
                })
            }
        });
    }
}
function getDetails(productId) {

    $.ajax({
        url: `/Customer/Home/Details?id=${productId}`,
        method: 'get',
        success: function (response) {
            window.open(`/Customer/Home/Details?id=${productId}`, '_blank');
        }
    });
}