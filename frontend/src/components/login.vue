<template>
    <div class="login-container">
        <h1 class="mb-4">Login</h1>
        <form @submit.prevent="handleLogin">
            <div>
                <input class="form-control" id="email" type="email" v-model="email" placeholder="Enter email" required />
            </div>
            <div>
                <input class="form-control" id="password" type="password" v-model="password" placeholder="Enter password" required />
            </div>
            <button class="form-control mb-4" type="submit">Login</button>
            <text type="button" @click="goToRegister">Register</text>
        </form>
    </div>

    <div v-if="loading" class="loading-overlay">
        <div class="spinner-border" role="status">
            <span class="visually-hidden"> Loading...</span>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';//reactive variable
    import { useRouter, useRoute } from 'vue-router';
    import { useToast } from "vue-toastification"; 

    const email = ref('');
    const password = ref('');
    const success = ref('');


    const router = useRouter();//to redirect to homepage after login
    const route = useRoute();//gives the current route object

    const toast = useToast();

    const loading = ref(false);

    onMounted(() => {
        if (route.query.verified) {
            success.value = "Email verified successfully.";
        }
    });


    const goToRegister = () => {
        router.push('/register');
    }

    const handleLogin = async () => {
        loading.value = true;

        try {
            const response = await fetch('http://localhost:5022/api/User/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'//sending JSON data
                },
                body: JSON.stringify({
                    email: email.value,
                    password: password.value
                })//convert to JSON
            });

            if (response.ok) {//was the response successful?
                const data = await response.json();//parse the JSON response, the userId we have sent in backend

                localStorage.setItem('userId', data.userId);
                localStorage.setItem('token', data.token);
                localStorage.setItem('firstName', data.firstName);
                localStorage.setItem('role', data.role);

                toast.success('Login successful!');

                if (data.role == "Admin") {
                    router.push('/admin-page');
                } else {
                    router.push('/user-page');
                }

            } else {
                const err = await response.text();
                toast.error(err || "Login failed");
            }
        } catch (err) {
            toast.error('Network error');//fetch failed
            router.push('/login');
        } finally {
            loading.value = false;
        }
    };
</script>

<style scoped>
    .login-container 
    {
        max-width: 400px;
        margin: 80px auto;
        padding: 2rem;
        border: 1px solid #ccc;
        border-radius: 8px;
    }

    input {
        width: 100%;
        padding: 0.5rem;
        margin-top: 0.25rem;
        margin-bottom: 1rem;
        border: 1px solid #aaa;
        border-radius: 4px;
    }

    button {
        width: 100%;
        padding: 0.5rem;
        background-color: #42b983;
        border: none;
        color: white;
        font-weight: bold;
        cursor: pointer;
    }

    button:hover {
            background-color: darkmagenta;
        }

    .error {
        color: red;
        margin-top: 1rem;
    }


    .loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(255, 255, 255, 0.7);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 2000;
    }
    
</style>