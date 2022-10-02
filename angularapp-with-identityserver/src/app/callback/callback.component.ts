import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import * as Oidc from 'oidc-client';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.css']
})
export class CallbackComponent implements OnInit {

  constructor(private router:Router) { }

  ngOnInit(): void {
    new Oidc.UserManager({response_mode:'query'}).signinRedirectCallback().then(() => {
      this.router.navigateByUrl('/');
    })
  }

}