export let activeToasts = [];

export function showToast(type, message1, message2, duration = 2000) {
 const toastContainer = document.createElement('div');
 toastContainer.classList.add('toast', type);

 let iconClass;
 switch(type) {
   case 'success':
     iconClass = 'fas fa-check';
     break;
   case 'error':
     iconClass = 'fas fa-times';
     break;
   case 'warning':
     iconClass = 'fas fa-exclamation';
     break;
   default:
     iconClass = 'fas fa-info';
 }

 toastContainer.innerHTML = `
   <div class="toast-content">
     <i class="${iconClass} check"></i>
     <div class="message">
       <span class="text text-1">${message1}</span>
       <span class="text text-2">${message2}</span>
     </div>
   </div>
   <i class="fas fa-times close"></i>
   <div class="progress"></div>
 `;

 const toastWrapper = document.querySelector('.toast-container') || createToastWrapper();
 toastWrapper.appendChild(toastContainer);

 const closeIcon = toastContainer.querySelector(".close");
 const progress = toastContainer.querySelector(".progress");

 activeToasts.push(toastContainer);
 updateToastPositions();

 toastContainer.classList.add("active");
 progress.classList.add("active");

 let timer1, timer2;

 timer1 = setTimeout(() => {
   toastContainer.classList.remove("active");
 }, duration);

 timer2 = setTimeout(() => {
   progress.classList.remove("active");
   toastWrapper.removeChild(toastContainer);
   activeToasts = activeToasts.filter(toast => toast !== toastContainer);
   updateToastPositions();
 }, duration + 300);

 closeIcon.addEventListener("click", () => {
   toastContainer.classList.remove("active");

   setTimeout(() => {
     progress.classList.remove("active");
     toastWrapper.removeChild(toastContainer);
     activeToasts = activeToasts.filter(toast => toast !== toastContainer);
     updateToastPositions();
   }, 300);

   clearTimeout(timer1);
   clearTimeout(timer2);
 });
}

export function createToastWrapper() {
 const toastWrapper = document.createElement('div');
 toastWrapper.classList.add('toast-container');
 document.body.appendChild(toastWrapper);
 return toastWrapper;
}

export function updateToastPositions() {
 activeToasts.forEach((toast, index) => {
   toast.style.top = `${index * ( 10)}px`;
 });
}

