var dotted = {
    page: null,
    resize: null,
    init: function () {
        // SET 
        if (document.querySelector('#lines') != null) {
            this.setLines();
        }
    },
    setLines: function () {
        this.canvas = document.querySelector('canvas');
        var ctx = this.canvas.getContext('2d'),
            color = '#ffffff';
        this.canvas.width = window.innerWidth;
        this.canvas.height = window.innerHeight;
        this.canvas.style.display = 'block';
        ctx.fillStyle = color;
        ctx.lineWidth = .2;
        ctx.strokeStyle = color;
        this.resize = function () {
            dotted.canvas.width = window.innerWidth;
            dotted.canvas.height = window.innerHeight;
            ctx.fillStyle = color;
            ctx.lineWidth = .2;
            ctx.strokeStyle = color;
        };
        window.addEventListener('resize', dotted.resize);
        var mousePosition = {
            x: 30 * dotted.canvas.width / 100,
            y: 30 * dotted.canvas.height / 100
        };
        var dots = {
            nb: parseInt(screen.width / 4.5),
            distance: 80,
            d_radius: 150,
            array: []
        };
        function Dot() {
            this.x = Math.random() * dotted.canvas.width;
            this.y = Math.random() * dotted.canvas.height;
            this.vx = -.5 + Math.random();
            this.vy = -.5 + Math.random();
            this.radius = Math.random();
        }
        Dot.prototype = {
            create: function () {
                ctx.beginPath();
                ctx.arc(this.x, this.y, this.radius, 0, Math.PI * 2, false);
                ctx.fill();
            }
        };
        Dot.animate = function () {
            var i, dot;
            for (i = 0; i < dots.nb; i++) {
                dot = dots.array[i];
                if (dot.y < 0 || dot.y > dotted.canvas.height) {
                    dot.vx = dot.vx;
                    dot.vy = - dot.vy;
                }
                else if (dot.x < 0 || dot.x > dotted.canvas.width) {
                    dot.vx = - dot.vx;
                    dot.vy = dot.vy;
                }
                dot.x += dot.vx;
                dot.y += dot.vy;
            }
        };
        Dot.line = function () {
            var i, j, i_dot, j_dot;
            for (i = 0; i < dots.nb; i++) {
                for (j = 0; j < dots.nb; j++) {
                    i_dot = dots.array[i];
                    j_dot = dots.array[j];
                    if ((i_dot.x - mousePosition.x) < dots.d_radius && (i_dot.y - mousePosition.y) < dots.d_radius && (i_dot.x - mousePosition.x) > - dots.d_radius && (i_dot.y - mousePosition.y) > - dots.d_radius) {
                        if ((i_dot.x - j_dot.x) < dots.distance && (i_dot.y - j_dot.y) < dots.distance && (i_dot.x - j_dot.x) > - dots.distance && (i_dot.y - j_dot.y) > - dots.distance) {
                            ctx.beginPath();
                            ctx.moveTo(i_dot.x, i_dot.y);
                            ctx.lineTo(j_dot.x, j_dot.y);
                            ctx.stroke();
                            ctx.closePath();
                        }
                    }
                }
            }
        };
        function createDots() {
            var i;
            ctx.clearRect(0, 0, dotted.canvas.width, dotted.canvas.height);
            if (dots.array.length < 1) {
                for (i = 0; i < dots.nb; i++) {
                    dots.array.push(new Dot());
                }
            }
            for (i = 0; i < dots.nb; i++) {
                var dot = dots.array[i];
                dot.create();
            }
            Dot.line();
            Dot.animate();
        }
        document.querySelector('#dotted').addEventListener('mousemove', function (e) {
            mousePosition.x = e.pageX;
            mousePosition.y = e.pageY;
        });
        document.querySelector('#dotted').addEventListener('mouseleave', function (e) {
            mousePosition.x = dotted.canvas.width / 2;
            mousePosition.y = dotted.canvas.height / 2;
        });
        this.interval = setInterval(createDots, 1000 / 30);
    },
    destroy: function () {
        if (this.interval) {
            clearInterval(this.interval);
        }
        if (dotted.resize) {
            window.addEventListener('resize', dotted.resize);
        }
    }
};
dotted.init();