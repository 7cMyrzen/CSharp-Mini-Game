// Ajout ou suppression de la classe active de display et de nav selon quel bouton a été cliqué
const navbtn_status = document.querySelector(".statusbtn");
const navbtn_shop = document.querySelector(".shopbtn");
const navbtn_debt = document.querySelector(".debtbtn");

// Display
const display_status = document.querySelector(".status");
const display_shop = document.querySelector(".shop");
const display_debt = document.querySelector(".debtdisplay");

// Event listeners
navbtn_status.addEventListener("click", () => {
    if (display_shop.classList.contains("active")) {
        display_shop.classList.remove("active");
        navbtn_shop.classList.remove("active");
    }
    if (!display_status.classList.contains("active")) {
        display_status.classList.add("active");
        navbtn_status.classList.add("active");
    }
    if (display_debt.classList.contains("active")) {
        display_debt.classList.remove("active");
        navbtn_debt.classList.remove("active");
    }
});

navbtn_shop.addEventListener("click", () => {
    if (display_status.classList.contains("active")) {
        display_status.classList.remove("active");
        navbtn_status.classList.remove("active");
    }
    if (!display_shop.classList.contains("active")) {
        display_shop.classList.add("active");
        navbtn_shop.classList.add("active");
    }
    if (display_debt.classList.contains("active")) {
        display_debt.classList.remove("active");
        navbtn_debt.classList.remove("active");
    }
});

navbtn_debt.addEventListener("click", () => {
    if (display_status.classList.contains("active")) {
        display_status.classList.remove("active");
        navbtn_status.classList.remove("active");
    }
    if (display_shop.classList.contains("active")) {
        display_shop.classList.remove("active");
        navbtn_shop.classList.remove("active");
    }
    if (!display_debt.classList.contains("active")) {
        display_debt.classList.add("active");
        navbtn_debt.classList.add("active");
    }
});

// URL de l'API
const apiUrl = "http://localhost:5105";
const API_URL = "http://localhost:5105/zoo";

// Fonction pour récupérer les données de l'API
async function fetchZooData() {
    try {
        const response = await fetch(`${API_URL}`, {
            method: "GET",
        });
        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            console.error("Erreur lors de la récupération des données:", response.status);
        }
    } catch (error) {
        console.error("Erreur avec l'API:", error);
    }
}

// Jour, Argent, Dette, Animaux, Terrain
const day = document.querySelector(".actual-day");
const money = document.querySelector(".actual-money");
const debt = document.querySelector(".actual-debt");
const debt2 = document.querySelector(".actual-debt2");
const animal = document.querySelector(".actual-animal-capacity");
const land = document.querySelector(".actual-land-capacity");
const visitors = document.querySelector(".actual-visitor");
const price = document.querySelector(".actual-price");
const animalnumber = document.querySelector(".actual-animal-number");
const animallist = document.querySelector(".animal-list");
const shopmoney = document.querySelector(".shop-money");
const shopmoney2 = document.querySelector(".shop-money2");
const animalshop = document.querySelector(".shop-animal");
const landprice = document.querySelector(".land-price");

// Quand la page est chargée on récupère les données de l'API pour l'afficher
window.addEventListener("load", async () => {
    const data = await fetchZooData();
    reloadData(data);
});

// Fonction pour mettre à jour les données affichées
function reloadData(data) {
    // Mise à jour des autres éléments
    day.textContent = data.jours;
    money.textContent = data.argent;
    debt.textContent = data.dette;
    var animaltext = (data.animaux.length * 5) / data.terrain * 100;
    animal.textContent = animaltext;
    land.textContent = data.terrain;
    visitors.textContent = data.nbVisiteurs;
    var pricetext = 10;
    price.textContent = pricetext;

    // Dictionnaire pour compter les animaux
    let dict = {};
    data.animaux.forEach(function (animal) {
        if (dict[animal.nom] === undefined) {
            dict[animal.nom] = 1;
        } else {
            dict[animal.nom] += 1;
        }
    });

    // Vider la liste des animaux avant de la remplir à nouveau
    animallist.innerHTML = "";

    // Génération du HTML pour chaque animal
    let innerhtml = "";
    for (let animal in dict) {
        let nom = animal;
        let nombre = dict[animal];
        innerhtml += `<div class="animal">
                        <div class="img"><img src="images/${nom}.jpg"></div>
                        <div class="name">${nom}</div>
                        <div class="quantity">X${nombre}</div>
                      </div>`;
    }
    animallist.innerHTML = innerhtml;

    // Mise à jour de l'argent dans la boutique et dans la dette
    shopmoney.textContent = data.argent;
    shopmoney2.textContent = data.argent;
    debt2.textContent = data.dette;
    // Mise à jour des prix du terrain
    priceOfLand =  data.terrainParExpansion * 40;
    landprice.textContent = priceOfLand;

    // Mise à jour des animaux dans la boutique
    let innerHTML2 = "";
    data.animauxDisponibles.forEach(function (animal) {
        let nom = animal.nom;
        let revenu = animal.revenuQuotidien;
        let prix = animal.prix;
        innerHTML2 += `<div class="animal">
                                <div class="img"><img src="images/${nom}.jpg"></div>
                                <span class="animal-income">Revenu : ${revenu}</span>
                                <div class="buy">
                                    <img src="images/10243319.png">
                                    <span class="animal-price" id="${nom}">${prix}</span>
                                </div>
                            </div>`;
    });
    animalshop.innerHTML = innerHTML2;

    // Ajouter des événements de clic sur les nouveaux boutons d'achat
    const buyButtons = document.querySelectorAll(".buy");
    buyButtons.forEach((button) => {
        button.addEventListener("click", async () => {
            const animalElement = button.closest(".animal");
            const animalName = animalElement.querySelector(".animal-price").id; // Nom depuis l'id
            const animalPrice = parseInt(animalElement.querySelector(".animal-price").textContent);
            const animalIncome = parseInt(
                animalElement.querySelector(".animal-income").textContent.replace("Revenu : ", "")
            );

            const animal = {
                nom: animalName,
                prix: animalPrice,
                revenuQuotidien: animalIncome,
            };

            // Envoyer la requête POST
            try {
                const response = await fetch(`${apiUrl}/acheter`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(animal),
                });
                if (response.ok) {
                    const updatedData = await fetchZooData();
                    reloadData(updatedData); // Recharger les données
                } else {
                    console.error("Erreur lors de l'achat:", response.status);
                }
            } catch (error) {
                console.error("Erreur avec la requête POST:", error);
            }
        });
    });

    if (data.dette === 0) {
        if (data.jours > data.maxJours) {
            window.location.href = "lose.html";
        } else {
            window.location.href = "win.html";
        }
    }
}

// Bouton pour récupérer les recettes de la journée
const recettebtn = document.getElementById("claim");
recettebtn.addEventListener("click", async () => {
    await fetch(`${apiUrl}/collecter`, { method: "POST" });
    const data = await fetchZooData();
    reloadData(data);
});

//Bouton pour acheter un terrain
const buylandbtn = document.querySelector(".add-land");
buylandbtn.addEventListener("click", async () => {
    await fetch(`${apiUrl}/agrandir`, { method: "POST" });
    const data = await fetchZooData();
    reloadData(data);
});

// Bouton pour rembourser la dette
const paydebtbtn = document.querySelector(".repay-btn");
paydebtbtn.addEventListener("click", async () => {
    // Récupérer la valeur entrée par l'utilisateur
    const moneyInput = document.querySelector(".repay-input");
    const value = parseInt(moneyInput.value);

    const debt = {
        montant: value,
    };

    // Envoyer la requête POST
    try {
        const response = await fetch(`${apiUrl}/rembourser`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(debt),
        });
        if (response.ok) {
            const data = await fetchZooData();
            reloadData(data);
        } else {
            console.error("Erreur lors du remboursement de la dette:", response.status);
        }
    } catch (error) {
        console.error("Erreur avec la requête POST:", error);
    }
    
});

