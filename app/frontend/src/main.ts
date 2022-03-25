import Vue from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";

import Vuetify from "vuetify";
import "vuetify/dist/vuetify.min.css";
Vue.use(Vuetify);
const opts = {
  theme: {
    dark: true,
  },
};
const vuetify = new Vuetify(opts);

import * as VueMenu from "@hscmap/vue-menu";
Vue.use(VueMenu);

Vue.config.productionTip = false;

new Vue({
  router,
  vuetify,
  store,
  render: (h) => h(App),
}).$mount("#app");
