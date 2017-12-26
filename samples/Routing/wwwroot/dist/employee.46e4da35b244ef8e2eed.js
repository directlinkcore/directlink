/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "/dist/";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 4);
/******/ })
/************************************************************************/
/******/ ({

/***/ 4:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

components.Employee = function (_React$Component) {
    _inherits(Employee, _React$Component);

    function Employee(props) {
        _classCallCheck(this, Employee);

        var _this = _possibleConstructorReturn(this, (Employee.__proto__ || Object.getPrototypeOf(Employee)).call(this, props));

        directlink.init(_this);
        _this.state.links = [{ name: 'Jennifer', href: 'employee/1' }, { name: 'John', href: 'employee/John' }, { name: 'Alice', href: 'employee/3' }, { name: 'Bill', href: 'employee/Bill' }, { name: 'Angela', href: 'employee/42' }];
        return _this;
    }

    _createClass(Employee, [{
        key: 'componentWillUnmount',
        value: function componentWillUnmount() {
            directlink.dispose(this);
        }
    }, {
        key: 'render',
        value: function render() {
            return React.createElement(
                'div',
                { className: 'card' },
                React.createElement(
                    'div',
                    { className: 'card-header d-inline' },
                    'Employee ',
                    this.state.args && React.createElement(
                        Link,
                        { href: '' },
                        'back'
                    )
                ),
                React.createElement(
                    'div',
                    { className: 'card-body' },
                    this.state.args && React.createElement(
                        'div',
                        null,
                        React.createElement(
                            'div',
                            { className: 'mb-3' },
                            'id = ',
                            React.createElement(
                                'div',
                                { className: 'badge badge-info mr-3' },
                                this.state.args.id || 'null'
                            ),
                            'name = ',
                            React.createElement(
                                'div',
                                { className: 'badge badge-info' },
                                this.state.args.name || 'null'
                            )
                        ),
                        React.createElement('img', { src: '/img/employees/' + (this.state.args.id || this.state.args.name) + '.png',
                            className: 'img-thumbnail rounded', style: { maxWidth: 200 } })
                    ),
                    !this.state.args && this.state.links.map(function (link) {
                        return React.createElement(
                            Link,
                            { key: link.name, href: link.href, className: 'nav-link' },
                            link.name
                        );
                    })
                )
            );
        }
    }]);

    return Employee;
}(React.Component);

/***/ })

/******/ });