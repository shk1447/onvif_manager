import Vue from "vue";
import VueRouter, { RouteConfig } from "vue-router";
import MainView from "../views/MainView.vue";
import SubView from "../views/SubView.vue";

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: "/",
    name: "Main",
    component: MainView,
  },
  {
    path: "/sub",
    name: "Sub",
    component: SubView,
  },
];

const router = new VueRouter({
  routes,
});

export default router;
