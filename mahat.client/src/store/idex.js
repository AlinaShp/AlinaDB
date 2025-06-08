import { createStore } from "vuex";


export default createStore({
  state: {
    user: null,
    themeMode: "light",
  },
  mutations: {
    setUser(state, user) {
      state.user = user;
    },
    setThemeMode(state, mode) {
      state.themeMode = mode;
      console.log(mode);
      localStorage.setItem("themeMode", mode);
    },
  },
  actions: {
    async changeUser(context, newUser) {
      context.commit("setUser", newUser);
    },
  },
  getters: {
    isAuthenticated: (state) => !!state.user,
  },
});
