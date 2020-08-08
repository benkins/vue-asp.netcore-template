<template>
  <div>
    <b-navbar toggleable="lg" type="dark" variant="info">
      <b-navbar-brand>Application</b-navbar-brand>

      <b-navbar-toggle target="nav-collapse"></b-navbar-toggle>

      <b-collapse id="nav-collapse" is-nav>
        <b-navbar-nav>
          <b-nav-item>
            <router-link to="/" class="nav-item">Home</router-link>
          </b-nav-item>
          <b-nav-item></b-nav-item>
        </b-navbar-nav>

        <b-navbar-nav class="ml-auto">
          <b-nav-item-dropdown right>
            <template v-slot:button-content>User</template>
            <div v-if="!this.isAuthenticated">
              <b-dropdown-item>
                <router-link to="/login" class="nav-item">Log in</router-link>
              </b-dropdown-item>
              <b-dropdown-item @click="blahBlah()">
                authentiated
              </b-dropdown-item>
            </div>
            <div v-if="this.isAuthenticated">
              <b-dropdown-item>
                <router-link to="/profile" class="nav-item">Profile</router-link>
              </b-dropdown-item>
              <b-dropdown-item>Sign Out</b-dropdown-item>
            </div>
          </b-nav-item-dropdown>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";

export default {
  computed: {
    ...mapGetters(["isAuthenticated"]),
  },

  methods: {
    ...mapActions(["logout"]),
    blahBlah() {
      console.log(this.isAuthenticated)
    }
  },
};
</script>

<style>
.nav-item {
  text-decoration: none;
  color: inherit;
}
</style>