import Vue from 'vue'
import App from './App.vue'
// import router from './routerManuaConfig'
import store from './store'
import router from './router/index'
import '../util/global'//全局
import './promissionRouter'//这里进行路由后台获取的模拟
import i18n from './lang' // Internationalization
import Cookies from 'js-cookie'

import ElementUI from "element-ui";
import "element-ui/lib/theme-chalk/index.css";

import Blob from '../util/Blob'
import Export2Excel from '../util/Export2Excel.js'

Vue.use(ElementUI, {
    size: Cookies.get('size') || 'medium', // set element-ui default size
    i18n: (key, value) => i18n.t(key, value)
});

//如果想使用 mock ，开启这两行即可，想看效果，看登录页的 mock登录功能
// import Mock from './mock'
// Mock.bootstrap();

import 'font-awesome/css/font-awesome.min.css' //引入font-awesome图标 //注释，因为跟下面的icons冲突，编译报错【Editby shaocx,2021-05-14】
import './icons';//icon,引入SVG图片 【Editby shaocx,2021-05-14】


Vue.config.productionTip = false

//引入公共变量文件 [EditBy shaocx,2021-05-14]
import global_ from './components/Global/Global.vue'//引用文件
Vue.prototype.GLOBAL = global_//挂载到Vue实例上面

new Vue({
    router,
    store,
    i18n,
    render: h => h(App)
}).$mount('#app')
