var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');

var nodemodules = "./node_modules";
var webroot = "./wwwroot/lib";

var paths = {
    bootstrap: {
        css: {
            src: nodemodules + '/bootstrap/dist/css/bootstrap.min.css',
            dest: webroot + '/bootstrap/dist/css'
        },
        js: {
            src: nodemodules + '/bootstrap/dist/js/bootstrap.bundle.min.js',
            dest: webroot + '/bootstrap/dist/js'
        }
    },
    jquery: {
        js: {
            src: nodemodules + '/jquery/dist/jquery.min.js',
            dest: webroot + '/jquery/dist'
        }
    },
    jquery_val: {
        js: {
            src: nodemodules + '/jquery-validation/dist/jquery.validate.min.js',
            dest: webroot + '/jquery-validation/dist'
        }
    },
    jquery_val_unob: {
        js: {
            src: nodemodules + '/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.min.js',
            dest: webroot + '/jquery-validation-unobtrusive/dist'
        }
    }
};

function clean() {
    return del([webroot]);
}

function copyStyles() {
    return gulp.src([paths.bootstrap.css.src], { sourcemaps: true })
        .pipe(gulp.dest(paths.bootstrap.css.dest));
}

function copyScripts(cb) {

    //return gulp.src([
    //    paths.bootstrap.js.src,
    //    paths.jquery.js.src,
    //    paths.jquery_val.js.src,
    //    paths.jquery_val_unob.js.src,
    //], { sourcemaps: true })
    //    .pipe(gulp.dest(webroot));

    var sources = [
        paths.bootstrap.js.src,
        paths.jquery.js.src,
        paths.jquery_val.js.src,
        paths.jquery_val_unob.js.src,
    ];

    var destinations = [
        paths.bootstrap.js.dest,
        paths.jquery.js.dest,
        paths.jquery_val.js.dest,
        paths.jquery_val_unob.js.dest,
    ];

    sources.forEach(function (value, index, array) {
        pipeline = gulp.src(value).pipe(gulp.dest(destinations[index]));
    });

    cb();
}

//function watch() {
//    gulp.watch(paths.scripts.src, scripts);
//    gulp.watch(paths.styles.src, styles);
//}

var build = gulp.series(clean, gulp.parallel(copyStyles, copyScripts));

/*
 * You can use CommonJS `exports` module notation to declare tasks
 */
exports.clean = clean;
exports.copyStyles = copyStyles;
exports.copyScripts = copyScripts;
/*exports.watch = watch;*/
exports.build = build;
/*
 * Define default task that can be called by just running `gulp` from cli
 */
exports.default = build;