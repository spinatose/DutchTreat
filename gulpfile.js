var gulp = require("gulp");
var uglify = require("gulp-uglify");
var concat = require("gulp-concat");

// minify javascript
function minifyjs() {
    return gulp.src(["wwwroot/js/**/*.js"])
        .pipe(uglify())
        .pipe(concat("dutchtreaties.min.js"))
        .pipe(gulp.dest("wwwroot/dist/"));
        
}

// minify css
function minifycss() {
    return gulp.src(["wwwroot/css/**/*.css"])
        //.pipe(uglify())
        .pipe(concat("dutchtreaties.min.css"))
        .pipe(gulp.dest("wwwroot/dist/"));

}

exports.minifyjs = minifyjs;
exports.minifycss = minifycss;

exports.default = gulp.parallel(minifyjs, minifycss);