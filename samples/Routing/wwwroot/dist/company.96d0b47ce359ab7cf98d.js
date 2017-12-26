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
/******/ 	return __webpack_require__(__webpack_require__.s = 2);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */,
/* 1 */,
/* 2 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

__webpack_require__(3);

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

components.Company = function (_React$Component) {
    _inherits(Company, _React$Component);

    function Company(props) {
        _classCallCheck(this, Company);

        var _this = _possibleConstructorReturn(this, (Company.__proto__ || Object.getPrototypeOf(Company)).call(this, props));

        directlink.init(_this);
        return _this;
    }

    _createClass(Company, [{
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
                    "Company ",
                    this.state.BackLink && React.createElement(
                        Link,
                        { href: this.state.BackLink.Href },
                        this.state.BackLink.Name
                    )
                ),
                React.createElement(
                    "div",
                    { className: "card-body" },
                    React.createElement(
                        "div",
                        { className: "mb-3" },
                        "Id = ",
                        React.createElement(
                            "div",
                            { className: "badge badge-primary mr-3" },
                            this.state.Id || 'null'
                        ),
                        "Name = ",
                        React.createElement(
                            "div",
                            { className: "badge badge-info mr-3" },
                            this.state.Name || 'null'
                        ),
                        "View = ",
                        React.createElement(
                            "div",
                            { className: "badge badge-secondary mr-3" },
                            this.state.View || 'null'
                        )
                    ),
                    this.state.Name && React.createElement("i", { className: "text-primary fab fa-" + this.state.Name.toLowerCase() + " fa-3x mb-3" }),
                    React.createElement(
                        "div",
                        { className: "row" },
                        this.state.Companies && React.createElement(
                            "div",
                            { className: "col-12 col-md-6 mb-3" },
                            React.createElement(
                                "div",
                                { className: "card" },
                                React.createElement(
                                    "div",
                                    { className: "card-header" },
                                    "Companies"
                                ),
                                React.createElement(
                                    "ul",
                                    { className: "list-group list-group-flush" },
                                    this.state.Companies.map(function (link) {
                                        return React.createElement(
                                            "li",
                                            { className: "list-group-item", key: link.Name },
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
                        this.state.Products && React.createElement(
                            "div",
                            { className: "col-12 col-md-6 mb-3" },
                            React.createElement(
                                "div",
                                { className: "card" },
                                React.createElement(
                                    "div",
                                    { className: "card-header" },
                                    "Products"
                                ),
                                React.createElement(
                                    "ul",
                                    { className: "list-group list-group-flush" },
                                    this.state.Products.map(function (link) {
                                        return React.createElement(
                                            "li",
                                            { className: "list-group-item", key: link.Name },
                                            React.createElement(
                                                Link,
                                                { href: link.Href, className: "nav-link" },
                                                link.Name
                                            )
                                        );
                                    })
                                )
                            )
                        )
                    ),
                    React.createElement(Render, { component: this.state.Component })
                )
            );
        }
    }]);

    return Company;
}(React.Component);

components.CompanyHome = function (_React$Component2) {
    _inherits(CompanyHome, _React$Component2);

    function CompanyHome(props) {
        _classCallCheck(this, CompanyHome);

        var _this2 = _possibleConstructorReturn(this, (CompanyHome.__proto__ || Object.getPrototypeOf(CompanyHome)).call(this, props));

        directlink.init(_this2);
        return _this2;
    }

    _createClass(CompanyHome, [{
        key: "componentWillUnmount",
        value: function componentWillUnmount() {
            directlink.dispose(this);
        }
    }, {
        key: "render",
        value: function render() {
            return React.createElement(
                "div",
                { className: "badge badge-secondary" },
                this.state.Path
            );
        }
    }]);

    return CompanyHome;
}(React.Component);

/***/ }),
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _possibleConstructorReturn(self, call) { if (!self) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return call && (typeof call === "object" || typeof call === "function") ? call : self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function, not " + typeof superClass); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, enumerable: false, writable: true, configurable: true } }); if (superClass) Object.setPrototypeOf ? Object.setPrototypeOf(subClass, superClass) : subClass.__proto__ = superClass; }

components.Product = function (_React$Component) {
    _inherits(Product, _React$Component);

    function Product(props) {
        _classCallCheck(this, Product);

        var _this = _possibleConstructorReturn(this, (Product.__proto__ || Object.getPrototypeOf(Product)).call(this, props));

        directlink.init(_this);
        return _this;
    }

    _createClass(Product, [{
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
                    "product id = ",
                    React.createElement(
                        "span",
                        { className: "badge badge-primary" },
                        this.state.args.id
                    )
                ),
                React.createElement(
                    "div",
                    { className: "card-body" },
                    React.createElement("img", { src: "/img/products/" + this.state.args.id + ".png" })
                )
            );
        }
    }]);

    return Product;
}(React.Component);

/***/ })
/******/ ]);