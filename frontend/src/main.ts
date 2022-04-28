/* eslint-disable @typescript-eslint/no-var-requires */
import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store';

import Vuetify from 'vuetify';
import { Iconfont } from 'vuetify/types/services/icons';
import 'vuetify/dist/vuetify.min.css';
import '@mdi/font/css/materialdesignicons.css';

Vue.use(Vuetify);
const opts = {
  icons: {
    iconfont: 'mdi' as Iconfont,
  },
  theme: {
    dark: true,
  },
};
const vuetify = new Vuetify(opts);

import * as VueMenu from '@hscmap/vue-menu';
Vue.use(VueMenu);

// 웹 빌드에 대한 분기 필요

import isElectron from 'is-electron';
if (isElectron()) {
  const ElectronAPI = require('./electron');
  Vue.prototype.$electron = ElectronAPI.default;
} else {
  Vue.prototype.$electron = null;
}

new Vue({
  router,
  vuetify,
  store,
  render: h => h(App),
}).$mount('#app');
