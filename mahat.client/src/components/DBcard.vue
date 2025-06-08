<template>
    <div class="container">
     <a class="card2" href="#">
         <div class="content">
 
     <h2>{{card.DatabaseName}}</h2>
     <div class="line"></div>
     <p class="small">ID: {{ card.DatabaseId }} </p>
     <p class="small">RecoveryModel: {{ card.RecoveryModel }} </p>
     <p class="small">State: {{ card.State }} </p>
     <p class="small">Compatibility: {{ card.CompatibilityLevel }} </p>
     <p class="small">Collation: {{ card.Collation }} </p>
     <button  @click="tableButtonClick" type="button" class="btn btn-outline-light">TABLES</button>
     <button  @click="backupButtonClick" type="button" class="btn btn-outline-light">BACKUP</button>
 
     <div class="go-corner" href="#">
       <div class="go-arrow">
         →
       </div>
       </div>
     </div>
   </a>
   </div>
 
   <BackupModal :databaseName="card.DatabaseName" ref="backupModalComponent"/>
 
 
 </template>
 <script>
 import BackupModal from "../components/BackupModal.vue";
 export default {
   components:{
     BackupModal
   },
   props: {
     card: {
       type: Object,
       required: true
     },
     // tableButtonClick: {
     //   type: Function,
     //   required: true
     // },
 
   }, methods: {
     tableButtonClick() {
       this.$emit('buttonClick');
     },
     backupButtonClick(){
       this.$refs.backupModalComponent.showModal();
     }
   }
 }
 </script>
 
 <style scoped>
 @import url('https://fonts.googleapis.com/css?family=Nunito:400,700');
 
 
 * {
   transition: all 0.3s ease-out;
 }
 
 html,
 body {
   height: 100%;
   font-family: "Nunito", sans-serif;
   font-size: 16px;
 }
 
 button{
   margin-left: 0.5vh;
 }
 .container {
   width: 100%;
   height: 100%;
   display: flex;
   flex-wrap: wrap;
   align-items: center;
   justify-content: center;
 }
 
 .content {
   flex-grow: 1; /* Растягивает содержимое, чтобы занять всё доступное пространство слева */
   padding-right: 20px; /* Отступ справа для разделения от уголка */
 }
 
 h2 {
   color: pink;
   font-size: 30px;
   line-height: 24px;
   font-weight: 700;
   margin-bottom: 4px;
   opacity: 1 !important;
   text-align: left; /* Выравнивание текста по левому краю */
 }
 
 p {
   font-size: 17px;
   font-weight: 500;
   line-height: 20px;
   color: #f3f3f3;
   text-align: left; /* Выравнивание текста по левому краю */
   &.small {
     font-size: 16px;
   }
 }
 
 .line {
   height: 1px;
   background-color: #fa9dea; /* Цвет линии */
   margin: 6px 0 10px; /* Отступы вокруг линии */
 }
 
 .go-corner {
   display: flex;
   align-items: center;
   justify-content: center;
   position: absolute;
   width: 32px;
   height: 32px;
   overflow: hidden;
   top: 0;
   right: 0;
   background-color: #fa9dea;
   border-radius: 0 4px 0 32px;
 }
 
 .go-arrow {
   margin-top: -4px;
   margin-right: -4px;
   color: white;
   font-family: courier, sans;
 }
 
 
 
 
 
 .card2 {
   display: block;
   top: 0px;
   position: relative;
   max-width: 262px;
   background-color: #404d7d;
 
   opacity: 0.95;
   border-radius: 4px;
   padding: 32px 24px;
   margin: 12px;
   text-decoration: none;
   z-index: 0;
   overflow: hidden;
   border: 1px solid #ff59bf9d;
   width: 30vh; /* Фиксированная ширина */
   height: 40vh;
 
   &:hover {
     transition: all 0.2s ease-out;
     box-shadow: 0px 4px 8px rgba(255, 19, 224, 0.2);
     top: -4px;
     border: 1px solid #cccccc;
     background-color: #05317cbe;
   }
 
   &:before {
     content: "";
     position: absolute;
     z-index: -1;
     top: -16px;
     right: -16px;
     background: #f89cf4;
     height: 32px;
     width: 32px;
     border-radius: 32px;
     transform: scale(2);
     transform-origin: 50% 50%;
     transition: transform 0.15s ease-out;
   }
 
   &:hover:before {
     transform: scale(2.15);
   }
 }
 
 
 </style>