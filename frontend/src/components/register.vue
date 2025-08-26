<template>
    <div class="register-container">
        <h1 class="mb-4">Register</h1>
        <form @submit.prevent="handleRegister">
            <div>
                <input class="form-control" id="email" name="email" type="email" v-model="email" placeholder="Email" required />
            </div>
            <div>
                <input class="form-control" id="firstName" name="firstName" type="text" v-model="firstName" placeholder="First name" required />
            </div>
            <div>
                <input class="form-control" id="lastName" name="lastName" type="text" v-model="lastName" placeholder="Last name" />
            </div>
            <div>
                <input class="form-control" id="password" name="password" type="password" v-model="password" placeholder="Password" required />
            </div>

            <div>
                <input class="form-control" id="password_2" type="password" v-model="password_2" placeholder="Enter password again" required />
            </div>
            <div class="mb-4">
                <label for="userRole" class="form-label"></label>
                <select id="userRole" name="userRole" v-model="userRole" class="form-select" required>
                    <option value="" disabled>Select a role</option>
                    <option value="Admin">Admin</option>
                    <option value="User">User</option>
                </select>
            </div>

            <button class="form-control mb-2" type="submit">Sign Up</button>
            <text type="button" @click="goToLogin">Login</text>
        </form>
        <p v-if="error" class="error">{{ error }}</p>
    </div>

    <div v-if="loading" class="loading-overlay">
        <div class="spinner-border" role="status">
            <span class="visually-hidden"> Loading...</span>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import { useRouter } from 'vue-router';

    const email = ref('');
    const firstName = ref('');
    const lastName = ref('');
    const password = ref('');
    const password_2 = ref('');
    const userRole = ref('');
    const error = ref('');

    const loading = ref(false);

    const router = useRouter();

    const goToLogin = () => {
        router.push('/login');
    }

    const handleRegister = async () => {
        error.value = '';
        loading.value = true;

        if (password.value != password_2.value) {
            error.value = "Passwords don't match!";
            return;
        }

        if (password.value.length < 6) {
            error.value = "Password must be at least 6 characters long.";
            return;
        }

        try {
            const response = await fetch('http://localhost:5022/api/User/register', {
                method: 'POST'
                , headers: {
                    'Content-Type': 'application/json'//sending JSON data
                },
                body: JSON.stringify({
                    email: email.value,
                    firstName: firstName.value,
                    lastName: lastName.value,
                    password: password.value,
                    userRole: userRole.value
                })//convert to JSON
            });

            if (!response.ok) {//was the response successful?
                error.value = 'Register failed';
            }

            const result = await response.text();//if successful, get the response text
            alert(result);//display result
            router.push('/login');//redirect to login page

        }
        catch (err) {
            error.value = 'Network error';//fetch failed
        } finally {
            loading.value = false;
        }
    }


</script>

<style scoped>
    .register-container {
    
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

    button:hover{
        background-color:darkmagenta;
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