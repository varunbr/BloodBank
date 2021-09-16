import { Component, OnInit } from '@angular/core';
import { Donor } from 'src/app/_modals/donor';
import { DonorService } from 'src/app/_services/donor.service';

@Component({
  selector: 'app-donors-list',
  templateUrl: './donors-list.component.html',
  styleUrls: ['./donors-list.component.css'],
})
export class DonorsListComponent implements OnInit {
  donors: Donor[] = [];

  constructor(private donorService: DonorService) {}

  ngOnInit() {
    this.donorService.getDonors().subscribe((response) => {
      this.donors = response;
    });
  }
}
