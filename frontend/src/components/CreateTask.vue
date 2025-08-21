<template>

    <header class="mb-8 navbar navbar-expand-lg navbar-light bg-light shadow-sm">
        <div class="container-fluid">
            <a class="navbar_brand fw_bold" href="/">
                <img src="../assets/images/homeimg.png" alt="home image" width="50" height="50" />
            </a>
            <button class="logout-btn btn " @click="handleLogout">Logout</button>
        </div>
    </header>

    <div class="create-task">

        <h2 class="mb-4">Create New Task</h2>
        <form @submit.prevent="handleCreate">
            <div>
                <input class="form-control" type="text" v-model="taskName" placeholder="Task Name" required />
            </div>
            <div class="mb-7">
                <textarea class="form-control" type="text" v-model="taskDescription" placeholder="Description" required></textarea>
            </div>
            <div class="mb-5">
                <label>Deadline</label>
                <input type="datetime-local" v-model="taskDeadline" required />
            </div>

            <div class="mb-5">
                <label>Choose a user to assign</label>
                <select v-model="selectedUserId" class="form-select" required>
                    <!--if the user picks the user with id=3, selectedUserId.value=3-->
                    <option disabled value="">Select a user</option><!--selectedUserId = "", daha seçim yapýlmadý -->
                    <option v-for="user in users" :key="user.usrId" :value="user.usrId">
                        {{ user.firstName }} {{ user.lastName }}
                    </option>
                </select>
            </div>

            <div class="tags-section">
                <label>Add tags</label>
                <div class="tags-input">
                    <input type="text" v-model="newTag" @keydown.enter.prevent="addTag" />
                </div>
            </div>
            <div class="mb-4 tags-list ">
                <span v-for="(tag, index) in tags"
                      :accesskey="index" class="tag-pill">
                    {{tag}}
                    <button type="button" class="rounded-circle remove-tag" @click="removeTag(index)">x</button>
                </span>
            </div>

            <button class="create-button" type="submit">Create Task</button>
            <p v-if="error" class="error">{{error}}</p>
        </form>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import { useRouter } from 'vue-router';
    import { apiFetch } from '../apiFetch';
    import { logout } from "../logout";

    const router = useRouter();

    const taskName = ref('');
    const taskDescription = ref('');
    const taskDeadline = ref('');
    const selectedUserId = ref('');
    const users = ref([]);
    const error = ref('');

    const tags = ref([]);
    const newTag = ref('');

    onMounted(async () => {//userlarý fetch lemek için
        try {
            const res = await fetch('http://localhost:5022/api/User/get_all');
            users.value = await res.json();
        } catch (err) {
            console.error('Error fetching users:');
        }
    });

    const addTag = () => {
        if (newTag.value.trim() !== '') {//check if the new tag is not empty
            tags.value.push(newTag.value.trim());//add to tags array
            newTag.value = '';//reset input box
        }
    };
    const removeTag = (index) => {
        tags.value.splice(index, 1);//remove one item
    };

    const handleCreate = async () => {
        error.value = '';

        try {
            const response = await apiFetch('http://localhost:5022/api/Task/create-task', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                }, body: JSON.stringify({
                    taskName: taskName.value,
                    taskDescription: taskDescription.value,
                    taskDeadline: taskDeadline.value,
                    usrId: selectedUserId.value, // Use the selected user ID
                    tags:tags.value
                })
            });

            if (!response.ok) {
                throw new Error('Failed to create task');
            }

            alert('Task created successfully!');

            //clear refs
            taskName.value = '';
            taskDescription.value = '';
            taskDeadline.value = '';
            selectedUserId.value = '';
            tags.value = [];

            router.push('/user-page');

        } catch (err) {
            error.value = err.message || 'Network error';
        }
    }
    const handleLogout = () => {
        logout();
        router.push('/');
    }

</script>

<style scoped>
    .create-task {
        max-width: 600px;
        margin: 0 auto;
        padding: 20px;
        background-color: #f9f9f9;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    .logout-btn{
        width:10%;
    }

    label{
        font-size:1.2rem;
    }

    input {
        width: 100%;
        padding: 0.5rem;
        margin-top: 0.25rem;
        margin-bottom: 1rem;
        border: 1px solid #aaa;
        border-radius: 4px;
    }

    textarea {
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

    .tag-pill {
        display: inline-flex;
        width:15%;
        padding:1px 1px 2px 5px;
        align-items: center;
        gap: 6px;
        font-size: 1rem;
        margin: 4px;
        background-color: #e0f7fa;
        border-radius: 20px; 
        font-weight: 550;
    }

    .remove-tag {
        background-color:transparent;
        color:black;
    }
    .error {
        color: red;
        margin-top: 1rem;
    }
</style>
