export default {
    namespaced: true,
    state: {
        profile: {}
    },
    getters: {
        isAuthenticated: state => state.profile.name && state.profile.email
    },
    mutations: {
        setProfile(state, profile) {
            state.profile = profile
        },
    },
    actions: {
        login(creds) {
            return fetch('https://localhost:5001/Authentication/authenticate', {
                method: 'post',
                headers: {
                    'Content-Type': 'multipart/form-data',
                    Accept: '*/*'
                },
                credentials: 'same-origin',
                body: JSON.stringify(creds),
            }).then(res => {
                commit('setProfile', res.data)
            })
        },
        logout({ commit }) {
            return fetch('https://localhost:5001/Authentication/logout', {
                method: 'post',
            }).then(() => {
                commit('setProfile', {});
            });
        },
        restoreContext({ commit }) {
            return fetch('https://localhost:5001/User/context', {
                method: 'get',
                headers: {
                    'Content-Type': 'application/json',
                },
            }).then(res => {
                commit('setProfile', res.json);
            });
        }

    }
}