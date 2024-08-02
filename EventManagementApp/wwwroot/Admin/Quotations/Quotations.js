import { showToast } from "../../Package/Package.js";
import { fetchData } from "../../Package/api.js";
import { toggleDisplay } from "../../Package/Domtools.js";

if (!localStorage.getItem("authToken")) {
  window.location.href = "/";
}

const hamburgerMenu = document.querySelector(".hamburger-menu");
const navLinks = document.querySelector(".nav-links");

hamburgerMenu.addEventListener("click", function () {
  navLinks.classList.toggle("active");
});

window.addEventListener("click", function (event) {
  if (
    !navLinks.contains(event.target) &&
    !hamburgerMenu.contains(event.target)
  ) {
    navLinks.classList.remove("active");
  }
});

function formatDate(dateString) {
  const options = {
    year: "numeric",
    month: "long",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  };
  return new Date(dateString).toLocaleDateString(undefined, options);
}

function getStatusClass(status) {
  switch (status.toLowerCase()) {
    case "initiated":
      return "status-initiated";
    case "responded":
      return "status-responded";
    case "accepted":
      return "status-accepted";
    case "completed":
      return "status-completed";
    case "denied":
      return "status-denied";
    default:
      return "";
  }
}

function createQuotationElement(quotation) {
  const div = document.createElement("div");
  div.className = "quotation";
  div.setAttribute("data-request-id", quotation.quotationRequestId);

  const statusClass = getStatusClass(quotation.quotationStatus);

  div.innerHTML = `
    <div>
        <h2>${quotation.eventCategory} <div class="status ${statusClass}">🏷️ ${
    quotation.quotationStatus
  }</div></h2>
        <div class="quotation-item">
            <div>
                <p><strong>Start Date:</strong> ${formatDate(
                  quotation.eventStartDate
                )}</p>
                <p><strong>End Date:</strong> ${formatDate(
                  quotation.eventEndDate
                )}</p>
                <p><strong>Special Instructions:</strong> ${
                  quotation.specialInstructions
                }</p>
                ${
                  quotation.foodPreference &&
                  quotation.foodPreference !== "NoFood"
                    ? `<p><strong>Catering Instructions:</strong> ${
                        quotation.cateringInstructions
                          ? quotation.cateringInstructions
                          : "Not provided"
                      }</p>`
                    : ""
                }
            </div>
            <div class="quotation-item-right">
                ${
                  quotation.foodPreference
                    ? `<p><strong>Food Preference:</strong> ${quotation.foodPreference}</p>`
                    : ""
                }
                <p><strong>Venue Type:</strong> ${quotation.venueType}</p>
                <p><strong>Location Details:</strong> ${
                  quotation.locationDetails
                }</p>
            </div>
        </div>
    </div>
    <div id="response-details-${
      quotation.quotationRequestId
    }" class="response-details"></div>
`;

  const handleClick = () => {
    showResponseDetails(quotation.quotationRequestId);
    div.removeEventListener("click", handleClick);
  };

  div.addEventListener("click", handleClick);
  return div;
}

async function showResponseDetails(requestId) {
  var response = await fetchData(`api/admin/requests/${requestId}`);
  const responseDetails = document.getElementById(
    `response-details-${requestId}`
  );

  if (response.quotationStatus === "Initiated") {
    responseDetails.innerHTML = `
            <button class="respond" onclick="showAdminResponseForm(${requestId})">Respond to Quotation</button>
        `;
  } else {
    response = response.quotationResponse;

    if (response) {
      responseDetails.style.display = "block";
      responseDetails.innerHTML = `
                <h2>Admin Response <div class="status ${getStatusClass(
                  response.requestStatus
                )}">${response.requestStatus}</div></h2>
                <p><strong>Response Message:</strong> ${
                  response.responseMessage
                }</p>
                ${
                  response.requestStatus === "Accepted"
                    ? `
                    <div class="quotation-item">
                        <div>
                            <p><strong>Quoted Amount:</strong> ${
                              response.quotedAmount
                            } ${response.currency}</p>
                        </div>
                    </div>
                    ${
                      response.clientResponse
                        ? `
                        <h2>Client Response<div class="status ${getStatusClass(
                          response.clientResponse.clientDecision
                        )}">${response.clientResponse.clientDecision}</div></h2>
                        ${
                          response.clientResponse.clientDecision === "Accepted"
                            ? `
${
  response.clientResponse.isPaid
    ? `<div class="paid"><img src="../../asserts/paid.png" alt="Paid" width="20">Paid</div>`
    : `<div>Waiting for payment ..`
}

                        `
                            : ""
                        }
                    `
                        : `
                        <div>Waiting for client response</div>
                    `
                    }
                `
                    : ""
                }
            `;

      setTimeout(() => {
        responseDetails.style.opacity = 1;
      }, 100);
    }
  }
}
window.showAdminResponseForm = showAdminResponseForm;
window.toggleresponsestatus = toggleresponsestatus;
window.closeResponseModal = closeResponseModal;
window.AddResponse = AddResponse;
window.newQuotation = newQuotation;
async function newQuotation() {
  if (
    document.getElementById("New Quotations").textContent == "New Quotations"
  ) {
    const newQuotation = await fetchData("api/admin/quotations?isnew=true");
    if (newQuotation <= 0) {
      showToast("success", "info", "No New Quotations");
      return;
    }
    quotations = newQuotation;
    renderQuotations();
    document.getElementById("New Quotations").textContent = "All Quotations";
  } else {
    quotations = await fetchData("api/admin/quotations");
    renderQuotations();
    document.getElementById("New Quotations").textContent = "New Quotations";
  }
}
function showAdminResponseForm(id) {
  document.getElementById("AddResponseform").reset();
  document.getElementById("AddResponseform").setAttribute("Requestid", id);
  document.getElementById("AddResponseModal").style.display = "flex";
}
function closeResponseModal() {
  document.getElementById("AddResponseModal").style.display = "none";
}

async function AddResponse(e) {
  e.preventDefault();
  const responsestatus = document.getElementById("response-status").value;
  const responseAmount = document.getElementById("quotedAmount").value;
  const responseMessage = document.getElementById("responseMessage").value;
  const quotationRequestId = document
    .getElementById("AddResponseform")
    .getAttribute("Requestid");

  if (responsestatus != "Denied" && responseAmount <= 0) {
    showToast("warning", "warning", "Amount not Be Negative");
    return;
  }
  await fetchData("api/quotation/response", "POST", {
    quotationRequestId: quotationRequestId,
    requestStatus: responsestatus,
    quotedAmount: parseInt(responseAmount),
    currency: "inr",
    responseMessage: responseMessage,
  });

  showResponseDetails(quotationRequestId);
  closeResponseModal();
}
var quotations;
let currentPage = 1;
const quotationsPerPage = 5;
document.addEventListener("DOMContentLoaded", async () => {
  quotations = await fetchData("api/admin/quotations");
  renderQuotations();
});
function renderPagination() {
  const paginationContainer = document.getElementById("pagination");
  paginationContainer.innerHTML = ``;
  const totalPages = Math.ceil(quotations.length / quotationsPerPage);

  for (let i = 1; i <= totalPages; i++) {
    const pageButton = document.createElement("button");
    pageButton.textContent = i;
    pageButton.addEventListener("click", () => {
      currentPage = i;
      renderQuotations();
      window.scrollTo({ top: 0, behavior: "smooth" });
    });

    if (i === currentPage) {
      pageButton.classList.add("active");
    }

    paginationContainer.appendChild(pageButton);
  }
}

function renderQuotations() {
  const quotationsContainer = document.getElementById("quotations");
  quotationsContainer.innerHTML = ``;
  const startIndex = (currentPage - 1) * quotationsPerPage;
  const endIndex = Math.min(startIndex + quotationsPerPage, quotations.length);

  for (let i = startIndex; i < endIndex; i++) {
    quotationsContainer.appendChild(createQuotationElement(quotations[i]));
  }

  renderPagination();
}

function toggleresponsestatus() {
  const foodValue = document.getElementById("response-status").value;
  const cateringDescription = document.querySelector(".toogle");
  if (foodValue === "Denied") {
    cateringDescription.style.display = "none";
  } else {
    cateringDescription.style.display = "block";
  }
}
