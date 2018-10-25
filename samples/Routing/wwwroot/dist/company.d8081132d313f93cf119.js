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
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
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
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./clientApp/company.jsx");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./clientApp/company.jsx":
/*!*******************************!*\
  !*** ./clientApp/company.jsx ***!
  \*******************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var product_jsx__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! product.jsx */ \"./clientApp/product.jsx\");\n/* harmony import */ var product_jsx__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(product_jsx__WEBPACK_IMPORTED_MODULE_0__);\nfunction _typeof(obj) { if (typeof Symbol === \"function\" && typeof Symbol.iterator === \"symbol\") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === \"function\" && obj.constructor === Symbol && obj !== Symbol.prototype ? \"symbol\" : typeof obj; }; } return _typeof(obj); }\n\nfunction _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError(\"Cannot call a class as a function\"); } }\n\nfunction _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if (\"value\" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }\n\nfunction _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }\n\nfunction _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === \"object\" || typeof call === \"function\")) { return call; } return _assertThisInitialized(self); }\n\nfunction _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }\n\nfunction _inherits(subClass, superClass) { if (typeof superClass !== \"function\" && superClass !== null) { throw new TypeError(\"Super expression must either be null or a function\"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }\n\nfunction _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }\n\nfunction _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError(\"this hasn't been initialised - super() hasn't been called\"); } return self; }\n\n\n\ncomponents.Company =\n/*#__PURE__*/\nfunction (_React$Component) {\n  _inherits(Company, _React$Component);\n\n  function Company(props) {\n    var _this;\n\n    _classCallCheck(this, Company);\n\n    _this = _possibleConstructorReturn(this, _getPrototypeOf(Company).call(this, props));\n    directlink.init(_assertThisInitialized(_assertThisInitialized(_this)));\n    return _this;\n  }\n\n  _createClass(Company, [{\n    key: \"componentWillUnmount\",\n    value: function componentWillUnmount() {\n      directlink.dispose(this);\n    }\n  }, {\n    key: \"render\",\n    value: function render() {\n      return React.createElement(\"div\", {\n        className: \"card\"\n      }, React.createElement(\"div\", {\n        className: \"card-header\"\n      }, \"Company \", this.state.BackLink && React.createElement(Link, {\n        href: this.state.BackLink.Href\n      }, this.state.BackLink.Name)), React.createElement(\"div\", {\n        className: \"card-body\"\n      }, React.createElement(\"div\", {\n        className: \"mb-3\"\n      }, \"Id = \", React.createElement(\"div\", {\n        className: \"badge badge-primary mr-3\"\n      }, this.state.Id || 'null'), \"Name = \", React.createElement(\"div\", {\n        className: \"badge badge-info mr-3\"\n      }, this.state.Name || 'null'), \"View = \", React.createElement(\"div\", {\n        className: \"badge badge-secondary mr-3\"\n      }, this.state.View || 'null')), this.state.Name && React.createElement(\"i\", {\n        className: \"text-primary fab fa-\".concat(this.state.Name.toLowerCase(), \" fa-3x mb-3\")\n      }), React.createElement(\"div\", {\n        className: \"row\"\n      }, this.state.Companies && React.createElement(\"div\", {\n        className: \"col-12 col-md-6 mb-3\"\n      }, React.createElement(\"div\", {\n        className: \"card\"\n      }, React.createElement(\"div\", {\n        className: \"card-header\"\n      }, \"Companies\"), React.createElement(\"ul\", {\n        className: \"list-group list-group-flush\"\n      }, this.state.Companies.map(function (link) {\n        return React.createElement(\"li\", {\n          className: \"list-group-item\",\n          key: link.Name\n        }, React.createElement(Link, {\n          href: link.Href,\n          className: \"nav-link\"\n        }, link.Name));\n      })))), this.state.Products && React.createElement(\"div\", {\n        className: \"col-12 col-md-6 mb-3\"\n      }, React.createElement(\"div\", {\n        className: \"card\"\n      }, React.createElement(\"div\", {\n        className: \"card-header\"\n      }, \"Products\"), React.createElement(\"ul\", {\n        className: \"list-group list-group-flush\"\n      }, this.state.Products.map(function (link) {\n        return React.createElement(\"li\", {\n          className: \"list-group-item\",\n          key: link.Name\n        }, React.createElement(Link, {\n          href: link.Href,\n          className: \"nav-link\"\n        }, link.Name));\n      }))))), React.createElement(Render, {\n        component: this.state.Component\n      })));\n    }\n  }]);\n\n  return Company;\n}(React.Component);\n\ncomponents.CompanyHome =\n/*#__PURE__*/\nfunction (_React$Component2) {\n  _inherits(CompanyHome, _React$Component2);\n\n  function CompanyHome(props) {\n    var _this2;\n\n    _classCallCheck(this, CompanyHome);\n\n    _this2 = _possibleConstructorReturn(this, _getPrototypeOf(CompanyHome).call(this, props));\n    directlink.init(_assertThisInitialized(_assertThisInitialized(_this2)));\n    return _this2;\n  }\n\n  _createClass(CompanyHome, [{\n    key: \"componentWillUnmount\",\n    value: function componentWillUnmount() {\n      directlink.dispose(this);\n    }\n  }, {\n    key: \"render\",\n    value: function render() {\n      return React.createElement(\"div\", {\n        className: \"badge badge-secondary\"\n      }, this.state.Path);\n    }\n  }]);\n\n  return CompanyHome;\n}(React.Component);\n\n//# sourceURL=webpack:///./clientApp/company.jsx?");

/***/ }),

/***/ "./clientApp/product.jsx":
/*!*******************************!*\
  !*** ./clientApp/product.jsx ***!
  \*******************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("function _typeof(obj) { if (typeof Symbol === \"function\" && typeof Symbol.iterator === \"symbol\") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === \"function\" && obj.constructor === Symbol && obj !== Symbol.prototype ? \"symbol\" : typeof obj; }; } return _typeof(obj); }\n\nfunction _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError(\"Cannot call a class as a function\"); } }\n\nfunction _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if (\"value\" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }\n\nfunction _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }\n\nfunction _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === \"object\" || typeof call === \"function\")) { return call; } return _assertThisInitialized(self); }\n\nfunction _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }\n\nfunction _inherits(subClass, superClass) { if (typeof superClass !== \"function\" && superClass !== null) { throw new TypeError(\"Super expression must either be null or a function\"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }\n\nfunction _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }\n\nfunction _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError(\"this hasn't been initialised - super() hasn't been called\"); } return self; }\n\ncomponents.Product =\n/*#__PURE__*/\nfunction (_React$Component) {\n  _inherits(Product, _React$Component);\n\n  function Product(props) {\n    var _this;\n\n    _classCallCheck(this, Product);\n\n    _this = _possibleConstructorReturn(this, _getPrototypeOf(Product).call(this, props));\n    directlink.init(_assertThisInitialized(_assertThisInitialized(_this)));\n    return _this;\n  }\n\n  _createClass(Product, [{\n    key: \"componentWillUnmount\",\n    value: function componentWillUnmount() {\n      directlink.dispose(this);\n    }\n  }, {\n    key: \"render\",\n    value: function render() {\n      return React.createElement(\"div\", {\n        className: \"card\"\n      }, React.createElement(\"div\", {\n        className: \"card-header\"\n      }, \"product id = \", React.createElement(\"span\", {\n        className: \"badge badge-primary\"\n      }, this.state.args.id)), React.createElement(\"div\", {\n        className: \"card-body\"\n      }, React.createElement(\"img\", {\n        src: \"/img/products/\".concat(this.state.args.id, \".png\")\n      })));\n    }\n  }]);\n\n  return Product;\n}(React.Component);\n\n//# sourceURL=webpack:///./clientApp/product.jsx?");

/***/ })

/******/ });