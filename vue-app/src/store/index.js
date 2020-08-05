import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

const store = new Vuex.Store({
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
      }).then((res) => {
        commit("setProfile", res.json);
      });
    },
  },
})

export default store;