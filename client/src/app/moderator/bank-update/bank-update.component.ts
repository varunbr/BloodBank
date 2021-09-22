import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Bank } from 'src/app/_modals/bank';
import { ModerateService } from 'src/app/_services/moderate.service';

@Component({
  selector: 'app-bank-update',
  templateUrl: './bank-update.component.html',
  styleUrls: ['./bank-update.component.css'],
})
export class BankUpdateComponent implements OnInit {
  bank: Bank;

  constructor(
    private route: ActivatedRoute,
    private moderateService: ModerateService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.getBank(params['id']);
    });
  }

  getBank(id: number) {
    this.moderateService.getBank(+id).subscribe((response) => {
      this.bank = JSON.parse(JSON.stringify(response));
    });
  }

  updateBlood(ngForm: NgForm) {
    this.moderateService
      .updateBloodData({ bankId: this.bank.id, groups: this.bank.bloodGroups })
      .subscribe((response) => {
        this.bank = JSON.parse(JSON.stringify(response));
        ngForm.reset();
        this.toastr.success('Updated');
      });
  }
}
