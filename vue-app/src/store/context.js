export default {
  namespaced: true,
  state: {
    profile: {},
  },
  getters: {
    isAuthenticated: (state) => state.profile.name && state.profile.email,
  },
  mutations: {
    setProfile(state, profile) {
      state.profile = profile;
    },
  },
  actions: {
    login({ commit }, credentials) {
      const resp = fetch(
        "https://localhost:44334/Authentication/authenticate",
        {
          method: "post",
          body: JSON.stringify(credentials),
          headers: {
            "Content-type": "application/json",
          },
        }
      ).then(() => {
        commit("setProfile", resp.data);
      });

      return resp;
    },

    // /* login({commit }, email, password) {
    //     return fetch('https://localhost:44334/Authentication/authenticate', {
    //         method: 'post',
    //         headers: {
    //             'Content-Type': 'text/plain',
    //             Accept: '*/*'
    //         },
    //         credentials: 'same-origin',

    //         body: '{email:' + email +', password:' + password + '}'
    //     }).then(res => {
    //         commit('setProfile', res.data)
    //     })
    // }, */
    logout({ commit }) {
      return fetch("https://localhost:44334/Authentication/logout", {
        method: "post",
      }).then(() => {
        commit("setProfile", {});
      });
    },
    restoreContext({ commit }) {
      return fetch("https://localhost:44334/User/context", {
        method: "get",
        headers: {
          "Content-Type": "application/json",
        },
      }).then((res) => {
        commit("setProfile", res.json);
      });
    },
  },
};
