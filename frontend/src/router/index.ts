import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import ClientView from '../views/ClientView.vue';
import InferenceManager from '../views/InferenceManager.vue';
import AnomalyManager from '../views/AnomalyManager.vue';

Vue.use(VueRouter);

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Client',
    component: ClientView,
  },
  {
    path: '/inference_manager',
    name: 'InferenceManager',
    component: InferenceManager,
  },
  {
    path: '/anomaly_manager',
    name: 'AnomalyManager',
    component: AnomalyManager,
  },
];

const router = new VueRouter({
  routes,
});

export default router;
