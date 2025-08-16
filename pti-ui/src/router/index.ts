import { createRouter, createWebHistory } from 'vue-router';

export default createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', name: 'dashboard', component: () => import('@/pages/Dashboard.vue') },
    { path: '/income', name: 'income', component: () => import('@/pages/Income.vue') },
    { path: '/summary', name: 'summary', component: () => import('@/pages/Summary.vue') },
  ]
});
