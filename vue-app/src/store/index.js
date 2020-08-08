import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

const store = new Vuex.Store({
  state: {
    profile: {
      name: '',
      email: ''
    }
  },
  getters: {
    isAuthenticated: (state) => state.profile.name && state.profile.email 
  },
  mutations: {
    setProfile(state, profile) {
      state.profile.name = profile.profile.name;
      state.profile.email = profile.profile.email;
    },
  },
  actions: {
    login({ commit }, profile) {
      commit("setProfile", profile)
    },
    logout({ commit }) {
      return fetch("https://localhost:5001/Authentication/logout", {
        method: "post",
      }).then(() => {
        commit("setProfile", {});
      });
    },
    restoreContext({ commit }) {
      return fetch("https://localhost:5001/Authentication/context", {
        method: "get",
        headers: {
          "Content-Type": "application/json",
        },
      })
      .then(response => {
        // The response is a Response instance.
        // You parse the data into a useable format using `.json()`
        return response.json();
      }).then(data => {
        commit("setProfile", data);
      });
    },
  },
})

export default store;