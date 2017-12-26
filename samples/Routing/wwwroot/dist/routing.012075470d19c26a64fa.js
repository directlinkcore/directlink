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
/******/ 	return __webpack_require__(__webpack_require__.s = 0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

__webpack_require__(1);

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

components.Routing = function (_React$Component) {
    _inherits(Routing, _React$Component);

    function Routing(props) {
        _classCallCheck(this, Routing);

        var _this = _possibleConstructorReturn(this, (Routing.__proto__ || Object.getPrototypeOf(Routing)).call(this, props));

        directlink.init(_this);
        return _this;
    }

    _createClass(Routing, [{
        key: "componentWillUnmount",
        value: function componentWillUnmount() {
            directlink.dispose(this);
        }
    }, {
        key: "render",
        value: function render() {
            return React.createElement(
                "div",
                null,
                React.createElement(
                    "nav",
                    { className: "navbar navbar-expand-lg navbar-light bg-light" },
                    React.createElement(
                        Link,
                        { className: "navbar-brand", href: "/" },
                        "Routing"
                    ),
                    React.createElement(
                        "button",
                        { className: "navbar-toggler", type: "button", "data-toggle": "collapse", "data-target": "#navbarSupportedContent", "aria-controls": "navbarSupportedContent", "aria-expanded": "false", "aria-label": "Toggle navigation" },
                        React.createElement("span", { className: "navbar-toggler-icon" })
                    ),
                    React.createElement(
                        "div",
                        { className: "collapse navbar-collapse", id: "navbarSupportedContent" },
                        React.createElement(
                            "ul",
                            { className: "navbar-nav mr-auto" },
                            this.state.Links.map(function (link) {
                                return React.createElement(
                                    "li",
                                    { key: link.Name, className: "nav-item" },
                                    React.createElement(
                                        Link,
                                        { href: link.Href, className: "nav-link" },
                                        link.Name
                                    )
                                );
                            })
                        )
                    )
                ),
                React.createElement(
                    "div",
                    { className: "jumbotron" },
                    React.createElement(Render, { component: this.state.Component })
                )
            );
        }
    }]);

    return Routing;
}(React.Component);

/***/ }),
/* 1 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

components.Home = function (_React$Component) {
    _inherits(Home, _React$Component);

    function Home(props) {
        _classCallCheck(this, Home);

        var _this = _possibleConstructorReturn(this, (Home.__proto__ || Object.getPrototypeOf(Home)).call(this, props));

        directlink.init(_this);
        return _this;
    }

    _createClass(Home, [{
        key: "componentWillUnmount",
        value: function componentWillUnmount() {
            directlink.dispose(this);
        }
    }, {
        key: "render",
        value: function render() {
            return React.createElement(
                "div",
                { className: "card" },
                React.createElement(
                    "div",
                    { className: "card-header" },
                    "Home"
                ),
                React.createElement(
                    "div",
                    { className: "card-body" },
                    "Navigate to ",
                    React.createElement(
                        Link,
                        { href: "/companies" },
                        "Companies"
                    ),
                    " or ",
                    React.createElement(
                        Link,
                        { href: "/employee" },
                        "Employees"
                    )
                )
            );
        }
    }]);

    return Home;
}(React.Component);

/***/ })
/******/ ]);