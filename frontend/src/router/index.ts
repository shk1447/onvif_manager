import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import ClientView from '../views/ClientView.vue';
import TrainView from '../views/TrainView.vue';

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Client',
    component: ClientView,
  },
  {
    path: '/train',
    name: 'Train',
    component: TrainView,
  },
];

const router = new VueRouter({
  routes,
});

export default router;
