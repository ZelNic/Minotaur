
function SendConfirmationCode(url) {
    Swal.fire({
        title: 'Отправить код подтверждения?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Да, отправить!',
        cancelButtonText: 'Отмена'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'POST'
            })
            EnterConfirmationCode(url);
        }
    })
}


function EnterConfirmationCode(url) {
    Swal.fire({
        title: 'Введите код подтверждения',
        text: 'Код отправлен клиенту',
        input: 'text',
        inputAttributes: {
            autocapitalize: 'off',
            pattern: '[0-9]+'
        },
        showCancelButton: true,
        confirmButtonText: 'Look up',
        showLoaderOnConfirm: true,
    }).then((result) => {
        if (result.isConfirmed) {
            return new Promise((resolve, reject) => {
                $.ajax({
                    url: url + "&confirmationCode=" + result.value,
                    type: 'POST',
                    success: function (response) {
                        resolve(response);
                    },
                    error: function (error) {
                        reject(error); // Промис завершился с ошибкой
                    }
                });
            });
        } else {
            return; // Вернуть пустое значение, если кнопка отмены была нажата
        }
    }).then((response) => {
        if (response) {
            Swal.fire({
                title: 'Код верный',
                icon: 'success'
            });
        }
    }).catch((error) => {
        Swal.fire({
            title: 'Ошибка при выполнении запроса',
            text: error.message,
            icon: 'error'
        });
    });
}









//Swal.fire(
//    'Отправлено!',
//    'Пользователь получил код подтверждения',
//    'success'
//)