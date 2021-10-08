<!-- PROJECT LOGO -->
<h1 align="center">The Blood Bank App</h1>

<!-- TABLE OF CONTENTS -->

<details open="open">
    <summary>Table of Contents</summary>
    <ol>
        <li>
            <a href="#about-the-project">About The Project</a>
        </li>
        <li>
            <a href="#features">Features</a>
            <ul>
                <li><a href="#member">Member</a></li>
                <li><a href="#bank-moderator">Bank Moderator</a></li>
                <li><a href="#bank-admin">Bank Admin</a></li>
                <li><a href="#moderator">Moderator</a></li>
                <li><a href="#admin">Admin</a></li>
            </ul>
        </li>
        <li>
            <a href="#built-with">Built With</a>
        </li>
        <li><a href="#contact">Contact</a></li>
        <li><a href="#acknowledgements">Acknowledgements</a></li>
        <li>
            <a href="#screenshots-and-working-process">Screenshots And Working Process</a>
            <ul>
                <li><a href="#donor-list">Donor List</a></li>
                <li><a href="#donor-detail">Donor Detail</a></li>
                <li><a href="#bank-list">Bank List</a></li>
                <li><a href="#moderate-list">Moderate List</a></li>
                <li><a href="#bank-update">Bank Update</a></li>
                <li><a href="#admin-list">Moderate List</a></li>
                <li><a href="#bank-edit">Bank Edit</a></li>
                <li><a href="#administration">Administration</a></li>
            </ul>
        </li>
    </ol>
</details>

<!-- ABOUT THE PROJECT -->

## About The Project

[![Product Name Screen Shot][product-screenshot]](https://blood-bank-demo.herokuapp.com/)

<p align="center">
    This Web App is about finding required blood group either by searching Donors or Blood Bank.
    <br />
    <a href="https://blood-bank-demo.herokuapp.com/">View Demo</a>
    ·
    <a href="#screenshots-and-working-process">View Screenshots</a>
</p>

## Features

The App has many user roles, based on the role users in, the required tabs will be added or removed.

There are five user roles for this App:

1. Member
2. Bank Moderator
3. Bank Admin
4. Moderator
5. Admin

#### Member

All the registered users will become part of this role. Member can search for Donors and Blood Banks. Members can edit and update their profile also they can configure whether they are available for blood donation.

#### Bank Moderator

Bank Moderator can update blood availability for the banks. A single user can be a Bank Moderator of many banks and a single bank can have many Bank Moderators.

#### Bank Admin

Bank Admin has all the access of what Bank Moderator has, and also they can add or remove Bank Moderators and Bank Admins, change bank photo. A single user can be Bank Admin of many banks and the single bank can have many Bank Admin.

#### Moderator

Moderators can create or update profiles of all Banks and also assign user roles for Banks.

#### Admin

Admin has top access to site, they can add or remove roles of Admin or Moderator.

## Built With

<p align="center">
    <a href="https://dotnet.microsoft.com/apps/aspnet/">
        <img src="images/dot-net-core.png" alt="ASP.NET" width="80" height="80">
    </a>
    <a href="https://angular.io/">
        <img src="images/angular.png" alt="Angular" width="80" height="80">
    </a>
    <a href="https://valor-software.com/ngx-bootstrap/#/">
        <img src="images/bootstrap.png" alt="ASP.NET" width="auto" height="80">
    </a>
</p>

|   # |                            Framework                            | Version |
| --: | :-------------------------------------------------------------: | :------ |
|   1 |     _ASP.NET_ [](https://dotnet.microsoft.com/apps/aspnet/)     | 5.0     |
|   2 |                _Angular_ [](https://angular.io/)                | 12.0    |
|   3 | _Ngx Bootstrap_ [](https://valor-software.com/ngx-bootstrap/#/) | 7.0     |

## Contact

<a href="https://varunbr.github.io">Contact</a>

## Acknowledgements

- [Cloudinary - Image Repository](https://cloudinary.com/)
- [AutoMapper - Dependency Injection](https://github.com/AutoMapper/AutoMapper)
- [Bootswatch - Themes for Bootstrap](https://bootswatch.com/)
- [Font Awesome - Icons](https://fontawesome.com/)
- [ngx-spinner - Loading spinner](https://www.npmjs.com/package/ngx-spinner)
- [ngx-toastr - Toastr Notification](https://www.npmjs.com/package/ngx-toastr)
- [ngx-timeago](https://www.npmjs.com/package/ngx-timeago)

## Screenshots And Working Process

### Donor List

- All registered users can search for donors by using Address, Blood Group and Age. All the eligible group donors for the selected group are shown. For example, if you are looking for an O+ group then O- group donors are also shown.
- Results are being paginated with 12 items per page.
- Each page is cached in-memory with applied filter and page number, so next time when you hit with the same filter and page number data from cache is used.
- Caching is done by reusable service which can be used for other page results.
- Any update to an item will automatically update all cached items with the same id.
- Donors shown here are based on their interest, They can always opt-out or opt-in from this anytime.

[![Donor List Screen Shot][donor-list]](https://blood-bank-demo.herokuapp.com/)

<hr/>

### Donor Detail

- A modal is shown with more details of the donor.

[![Donor Detail Screen Shot][donor-detail]](https://blood-bank-demo.herokuapp.com/)

<hr/>

### Bank List

- All registered users can search for blood banks by using Address, Blood Group and Bank Name.

[![Bank List Screen Shot][bank-list]](https://blood-bank-demo.herokuapp.com/)

<hr/>

### Bank Detail

- A modal is shown with more details of the bank.

[![Bank Detail Screen Shot][bank-detail]](https://blood-bank-demo.herokuapp.com/)

<hr/>

### Moderate List

- This page is shown only for users with role Bank Admin and Bank Moderator.
- User should be in role Bank Admin or Bank Moderator of each bank to be shown.

[![Moderate List Screen Shot][moderate-list]](https://blood-bank-demo.herokuapp.com/)

<hr/>

### Bank Update

- Bank Admin or Bank Moderator can view this page from Moderate List page.
- Bank Moderator can only update blood data of bank where as Bank Admin can change roles for bank and update bank photo.

[![Bank Update Screen Shot][bank-update]](https://blood-bank-demo.herokuapp.com/)

<hr/>

### Admin List

- Moderator can view this page for moderating Banks.
- They can search for banks or register new banks from this page.

[![Admin List Screen Shot][admin-list]](https://blood-bank-demo.herokuapp.com/)

<hr/>

### Bank Edit

- Moderator can edit all the bank details from this page.
- They can modify bank user roles.

[![Bank Edit Screen Shot][bank-edit]](https://blood-bank-demo.herokuapp.com/)

<hr/>

### Administration

- This page is for Admin which has highest level of access.
- Admin can add or remove roles of Moderator or Admin itself.

[![Administration Screen Shot][administration]](https://blood-bank-demo.herokuapp.com/)

<hr/>

<!-- MARKDOWN LINKS & IMAGES -->

[product-screenshot]: images/home.jpeg
[donor-list]: images/donor-list.jpeg
[donor-detail]: images/donor-detail.png
[bank-list]: images/bank-list.jpeg
[bank-detail]: images/bank-detail.png
[moderate-list]: images/moderate-list.jpeg
[bank-update]: images/bank-update.jpeg
[admin-list]: images/admin-list.jpeg
[bank-edit]: images/bank-edit.jpeg
[administration]: images/super-admin.jpeg
