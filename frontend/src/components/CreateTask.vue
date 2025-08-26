<template>
    <div class="task-creation-page">

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
                        <option v-for="user in users"
                                :key="user.usrId"
                                :value="user.usrId">
                            {{ user.firstName }} {{ user.lastName }}
                        </option>

                    </select>
                </div>

                <div class="tags-section">
                    <label>Add tags</label>
                    <div class="tags-input position-relative">
                        <input type="text"
                               v-model="newTag"
                               @focus="showAllTags"
                               @input="filterTags"
                               @blur="closeMenu"
                               @keydown.enter.prevent="addTag" />
                        <ul v-if="filteredTags.length" class="tag-dropdown">
                            <li v-for="(tag, index) in filteredTags"
                                :key="index"
                                @click="selectTag(tag)">
                                {{ tag }}
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="mb-4 tags-list ">
                    <span v-for="(tag, index) in tags"
                          :accesskey="index" class="tag-pill">
                        {{tag}}
                        <button type="button" class="rounded-circle remove-tag" @click="removeTag(index)">x</button>
                    </span>
                </div>

                <button class="btn create-button" type="submit">Create Task</button>
                <p v-if="error" class="error">{{error}}</p>
            </form>
        </div>
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
    const selectedUserId = ref(null);
    const users = ref([]);
    const error = ref('');

    const tags = ref([]);
    const allTags = ref([]);
    const newTag = ref('');
    const filteredTags = ref([]);

    onMounted(async () => {//userlarý fetch lemek için
        try {
            const resUser = await fetch('http://localhost:5022/api/User/get_all');
            users.value = await resUser.json();
            users.value = users.value.filter(user => user.userRole !== 'Admin');

            const resTag = await fetch('http://localhost:5022/api/Task/get-all-tags');
            allTags.value = await resTag.json();
        } catch (err) {
            console.error('Error fetching users or tags:', err);
        }
    });

    const addTag = async () => {
        const tagName = newTag.value.trim();
        if (!tagName) return;

        try {
            const response = await apiFetch('http://localhost:5022/api/Task/add-tag', {
                method: 'POST',
                headers: {
                    "Authorization": `Bearer ${localStorage.getItem("token")}`,
                    'Content-Type': 'application/json',
                }, body: JSON.stringify({
                    tagName: tagName
                })
            });

            if (!response.ok) {
                throw new Error('Failed to add tag');
            }

            const tag = await response.json();
            const tagString = typeof tag === "string" ? tag : tag.tagName;
            tags.value.push(tagString);

            newTag.value = '';


        } catch (err) {
            error.value = err.message || 'Network error';
        }


    };

    const removeTag = (index) => {
        tags.value = tags.value.filter((_, i) => i !== index);
    };

    const closeMenu = () => {
        setTimeout(() => {
            filteredTags.value = [];
        }, 200);
    };

    const filterTags = () => {
        const input = newTag.value.toLowerCase();
        if (!input) {
            showAllTags();
            return;
        }

        filteredTags.value = allTags.value.filter(tag => tag.toLowerCase().startsWith(input) && !tags.value.includes(tag));
    };

    const selectTag = (tag) => {
        const tagString = typeof tag === "string" ? tag : tag.tagName;
        tags.value.push(tagString);
        newTag.value = '';
        filteredTags.value = [];
    };

    const showAllTags = () => {
        filteredTags.value = allTags.value;
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
                    tags: tags.value
                })
            });

            console.log(response.value);

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
    .task-creation-page {
        background-image: url('../assets/images/background-user.jpg');
        background-size: cover;
        min-height: 100vh;
        padding: 20px;
    }

    .create-task {
        max-width: 600px;
        margin: 0 auto;
        padding: 20px;
        background-color: #f9f9f9;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }

    .logout-btn {
        width: 10%;
    }

    label {
        font-size: 1.2rem;
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

    .tag-dropdown {
        position: absolute;
        top: 100%;
        left: 0;
        right: 0;
        max-height: 150px;
        overflow-y: auto;
        background: white;
        border: 1px solid #ccc;
        border-radius: 4px;
        list-style: none;
        margin: 0;
        padding: 0;
        z-index: 10;
    }

        .tag-dropdown li {
            padding: 8px 12px;
            cursor: pointer;
        }

            .tag-dropdown li:hover {
                background: #eee;
            }

    .tag-pill {
        display: inline-flex;
        width: 15%;
        padding: 1px 1px 2px 5px;
        align-items: center;
        gap: 6px;
        font-size: 1rem;
        margin: 4px;
        background-color: #e0f7fa;
        border-radius: 20px;
        font-weight: 550;
    }

    .remove-tag {
        background-color: transparent;
        color: black;
    }

        .remove-tag:hover {
            background-color: transparent;
        }

    .error {
        color: red;
        margin-top: 1rem;
    }
</style>
