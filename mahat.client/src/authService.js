// // authService.js
// import { UserManager, WebStorageStateStore } from 'oidc-client';
// import { authConfig } from './authConfig';
// import { useStore } from 'vuex'
// const userManager = new UserManager({
//     authority: 'https://projectadfs.excelapp.local/adfs',
//     client_id: '8eccbd3e-eba4-40b6-bce5-37022d1a2467',
//     redirect_uri: 'http://sqlapi.excelapp.local/callback',
//     response_type: 'id_token token',
//     scope: 'openid profile',
//     post_logout_redirect_uri: 'http://sqlapi.excelapp.local',

//     userStore: new WebStorageStateStore({ store: window.localStorage })
// });

// async function handleAuthentication() {
//     const user = await userManager.getUser();
//     if (!user || user.expired) {
//       await userManager.signinRedirect();
//     } else {
//       // User is authenticated, save access token to localStorage
//       localStorage.setItem('access_token', user.access_token);
//     }
//   }
  

// userManager.events.addUserLoaded(user => {
//     console.log('User loaded:', user);
//     if (user && user.access_token) {
//         const store = useStore()
//         store.commit('setAccessToken', user.access_token);
//         console.log(user.access_token);
//     }
// });

// userManager.events.addUserUnloaded(() => {
//     console.log('User unloaded');
// });

// export function getUser() {
//     return userManager.getUser();
// }

// export function signinRedirect() {
//     return userManager.signinRedirect();
// }

// export function signoutRedirect() {
//     return userManager.signoutRedirect();
// }

// export function signinRedirectCallback() {
//     return userManager.signinRedirectCallback();
// }

// export function signoutRedirectCallback() {
//     return userManager.signoutRedirectCallback();
// }

// export default userManager;