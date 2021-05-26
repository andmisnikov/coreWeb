var UserChart = new function () {
    this.render = function () {
        fetch('User/UsersRegisteredPerDay').then(function (response) {
            return response.json();
        }).then(result => {
            var data = {
                datasets: [{
                    label: 'Users registered per day',
                    backgroundColor: 'rgb(255, 99, 132)',
                    borderColor: 'rgb(255, 99, 132)',
                    data: result
                }]
            };
            var config = {
                type: 'line',
                data,
                options: {
                    events: ['mousemove', 'mouseout', 'touchstart', 'touchmove'],
                    parsing: {
                        xAxisKey: 'day',
                        yAxisKey: 'usersRegistered'
                    },
                    scales: {
                        y: {
                            min: 0
                        },
                        x: {
                            //remove time
                        }
                    }
                }
            };
            var myChart = new Chart(
                document.getElementById('myChart'),
                config
            );
        }).catch(function (err) {
            console.log('Fetch problem: ' + err.message);
        });
    };
}