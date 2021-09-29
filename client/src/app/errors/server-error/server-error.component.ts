import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css'],
})
export class ServerErrorComponent implements OnInit {
  message;
  details;
  constructor(private route: Router) {
    const state = this.route.getCurrentNavigation()?.extras?.state;
    if (state) {
      this.message = state?.error?.message;
      this.details = state?.error?.details;
    }
  }

  ngOnInit(): void {}
}
