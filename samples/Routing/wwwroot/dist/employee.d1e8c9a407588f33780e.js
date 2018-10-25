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
/******/ 	return __webpack_require__(__webpack_require__.s = "./clientApp/employee.jsx");
/******/ })
/************************************************************************/
/******/ ({

/***/ "./clientApp/employee.jsx":
/*!********************************!*\
  !*** ./clientApp/employee.jsx ***!
  \********************************/
/*! no static exports found */
/***/ (function(module, exports) {

eval("function _typeof(obj) { if (typeof Symbol === \"function\" && typeof Symbol.iterator === \"symbol\") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === \"function\" && obj.constructor === Symbol && obj !== Symbol.prototype ? \"symbol\" : typeof obj; }; } return _typeof(obj); }\n\nfunction _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError(\"Cannot call a class as a function\"); } }\n\nfunction _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if (\"value\" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }\n\nfunction _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }\n\nfunction _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === \"object\" || typeof call === \"function\")) { return call; } return _assertThisInitialized(self); }\n\nfunction _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }\n\nfunction _inherits(subClass, superClass) { if (typeof superClass !== \"function\" && superClass !== null) { throw new TypeError(\"Super expression must either be null or a function\"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }\n\nfunction _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }\n\nfunction _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError(\"this hasn't been initialised - super() hasn't been called\"); } return self; }\n\ncomponents.Employee =\n/*#__PURE__*/\nfunction (_React$Component) {\n  _inherits(Employee, _React$Component);\n\n  function Employee(props) {\n    var _this;\n\n    _classCallCheck(this, Employee);\n\n    _this = _possibleConstructorReturn(this, _getPrototypeOf(Employee).call(this, props));\n    directlink.init(_assertThisInitialized(_assertThisInitialized(_this)));\n    _this.state.links = [{\n      name: 'Jennifer',\n      href: 'employee/1'\n    }, {\n      name: 'John',\n      href: 'employee/John'\n    }, {\n      name: 'Alice',\n      href: 'employee/3'\n    }, {\n      name: 'Bill',\n      href: 'employee/Bill'\n    }, {\n      name: 'Angela',\n      href: 'employee/42'\n    }];\n    return _this;\n  }\n\n  _createClass(Employee, [{\n    key: \"componentWillUnmount\",\n    value: function componentWillUnmount() {\n      directlink.dispose(this);\n    }\n  }, {\n    key: \"render\",\n    value: function render() {\n      return React.createElement(\"div\", {\n        className: \"card\"\n      }, React.createElement(\"div\", {\n        className: \"card-header d-inline\"\n      }, \"Employee \", this.state.args && React.createElement(Link, {\n        href: ''\n      }, \"back\")), React.createElement(\"div\", {\n        className: \"card-body\"\n      }, this.state.args && React.createElement(\"div\", null, React.createElement(\"div\", {\n        className: \"mb-3\"\n      }, \"id = \", React.createElement(\"div\", {\n        className: \"badge badge-info mr-3\"\n      }, this.state.args.id || 'null'), \"name = \", React.createElement(\"div\", {\n        className: \"badge badge-info\"\n      }, this.state.args.name || 'null')), React.createElement(\"img\", {\n        src: \"/img/employees/\".concat(this.state.args.id || this.state.args.name, \".png\"),\n        className: \"img-thumbnail rounded\",\n        style: {\n          maxWidth: 200\n        }\n      })), !this.state.args && this.state.links.map(function (link) {\n        return React.createElement(Link, {\n          key: link.name,\n          href: link.href,\n          className: \"nav-link\"\n        }, link.name);\n      })));\n    }\n  }]);\n\n  return Employee;\n}(React.Component);\n\n//# sourceURL=webpack:///./clientApp/employee.jsx?");

/***/ })

/******/ });