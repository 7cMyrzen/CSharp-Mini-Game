# Projet C#

<img alt="C#" src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white"/><img alt="JavaScript" src="https://img.shields.io/badge/javascript-%23323330.svg?&style=for-the-badge&logo=javascript&logoColor=%23F7DF1E"/>
<img alt="HTML5" src="https://img.shields.io/badge/html5-%23E34F26.svg?&style=for-the-badge&logo=html5&logoColor=white"/>
<img alt="CSS3" src="https://img.shields.io/badge/css3-%231572B6.svg?&style=for-the-badge&logo=css3&logoColor=white"/>

## Lancement du jeu :

- Télecharger le repo.
- Aller à la racine du dossier.
- Lancer la commande ``dotnet run``.
- Ouvrir la page internet ``localhost:5105``.

## But du jeu :

- Rembourser la dette dans l'onglet ``Rembourser`` sous 50 jours.

## Comment jouer :

### Page Statut :

- Jour indique le jour dans le jeu.
- Argent indique la somme d'argent possédé.
- Dette indique la dette à rembourser avant les 50 jours.
- Animaux indique le pourcentage d'animaux. Une fois atteint 100% vous ne pourrez plus en acheter.
- Terrain indique le pourcentage de terrain acquis.
- Dans animaux possédés vous trouverez quels sont vos animaux et combien vous en avez.
- Dans visiteurs, vous aurez le nombre de visiteurs dans le zoo par jours ( il augmente en agrandissant le terrain ).
- Le prix d'entrée est le prix que chaque visiteur paye par jour.
- Enfin le bouton ``Récupérer l'argent quotidien`` permet de récupérer l'argent gagné grâce aux animaux et le total des entrées. Il va ensuite faire passer jour à jour+1.

### Page Boutique : 

- Consulter votre argent
- Acheter des animaux ( l'argent qu'ils génèrent par jour est affiché au dessus du bouton pour acheter ).
- Acheter un terrain ( 2 fois max.). Permet de faire augmenter le nombre d'espèce possible dans le zoo et la capacité d'accueil de visiteur.

### Page Rembourser :

- Consulter cotre argent.
- Consulter la dette pour rembourser en indiquant le montant dans le champ d'entrée avant de cliquer sur ``Rembourser``.
