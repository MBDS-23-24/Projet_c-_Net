# Librairiz
*Pour tourner l'ensemble du projet référer vous a la branche 'master'*

## API

Fonctionnalités développées : 
1. Récupérer un livre avec son contenu : /book/{id}
2. Lister les genres disponibles : /genre
3. Lister les livres sans le contenu
   * Le résultat est paginé
   *  La recherche est faite en spécifiant un genre
   *  La réponse contient un header ayant l'index de début et de fin des livres ainsi que le nombre total de livres


## Administration

Fonctionnalités développées : 
### Un utilisateur peut : 
1. ajouter des livres dans la bibliothèque
2. supprimer des livres de la bibliothèque
3. Consulter la liste de tous les livres
4. Consulter la liste de tous les genres
5. Ajouter de nouveaux genres
6. Modifier un livre existant
7. Ajouter un livre avec plusieurs auteurs
8. Filtrer dans la liste des livres par auteur/genre
9. Voir le nombre total de livre disponible (Interface *Statistiques*)
10. Voir le nombre de livre par auteur
11. Voir le nombre maximum, minmum, median et moyen de mots d'un livre


## Application Windows (WPF.READER)

Fonctionnalités développées : 
1. Une page de garde avec un header contenant le titre de l'application, la liste des livres
2. Des appels vers l'API pour récupérer les données suivantes : la liste des livres, les détails d'un livre spécifique, et la lecture du livre demandé
3. Des boutons permettant de fluidifier la navigation sur l'application en passant d'une vue à une autre par l'exploitation du "GoBack" dans la navigation


## Difficultés rencontrés :

Dans la partie *Application Windows*, on n'a pas pu récupérer les auteurs et les genres pour chaque livre respectif 


## Contributeurs : 
1. Yehoudi VINCENT
2. Dounia ZOUBID
3. Elodie BANTOS
4. Abdenour ACHOURI


