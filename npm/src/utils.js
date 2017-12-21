let chars = '0123456789abcdef'.split('');

let getGuid = () => {
    let uuid = [],
        rnd = Math.random,
        r;
    uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
    uuid[14] = '4';

    for (let i = 0; i < 36; i++) {
        if (!uuid[i]) {
            r = 0 | rnd() * 16;
            uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r & 0xf];
        }
    }

    return uuid.join('');
};

let getScript = (url) => new Promise((resolve, reject) => {
    let script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = url;
    script.addEventListener('load', () => resolve(script), false);
    script.addEventListener('error', () => reject(script), false);
    document.body.appendChild(script);
});

let getStyle = (url) => new Promise((resolve, reject) => {
    let link = document.createElement('link');
    link.rel = 'stylesheet';
    link.href = url;
    link.addEventListener('load', () => resolve(link), false);
    link.addEventListener('error', () => reject(link), false);
    document.body.appendChild(link);
});

let getClassName = (instance) => {
    if (instance.constructor.name) {
        return instance.constructor.name;
    }
    const regex = new RegExp(/^\s*function\s*(\S*)\s*\(/);
    let getClassName = instance => instance.constructor.toString().match(regex)[1];
    return getClassName(instance);
};

class DeferredPromise {
    constructor(executor) {
        this._promise = new Promise((resolve, reject) => {
            this.resolve = resolve;
            this.reject = reject;
            executor(resolve, reject);
        });
        this.then = this._promise.then.bind(this._promise);
        this.catch = this._promise.catch.bind(this._promise);
    }
}

module.exports = {
    getGuid,
    getScript,
    getStyle,
    getClassName,
    DeferredPromise
};