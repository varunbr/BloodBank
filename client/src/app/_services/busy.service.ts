import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  requestCount = 0;

  constructor(private spinner: NgxSpinnerService) {}

  busy() {
    this.requestCount++;
    this.spinner.show(undefined, {
      type: 'timer',
      bdColor: 'rgba(255,255,255,0.5)',
      color: '#333333',
    });
  }

  idle() {
    this.requestCount--;
    if (this.requestCount <= 0) {
      this.spinner.hide();
    }
  }
}
