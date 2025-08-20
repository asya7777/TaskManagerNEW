import { createRouter, createWebHistory } from 'vue-router'

import LoginPage from '../components/login.vue'
import RegisterPage from '../components/register.vue'
import HomePage from '../components/homepage.vue'
import CreateTask from '../components/CreateTask.vue'
import AdminPage from '../components/adminPage.vue'
import UserPage from '../components/userPage.vue'

const routes = [
    {
        path: '/login',
        component: LoginPage
    }
    ,{
        path: '/register',
        component: RegisterPage
    }
    , {
        path: '/',
        component: HomePage
    }
    , {
        path: '/create-task', 
        component: CreateTask
    }
    , {
        path: '/admin-page',
        component: AdminPage
    }
    , {
        path: '/user-page',
        component:UserPage
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes
});

export default router;