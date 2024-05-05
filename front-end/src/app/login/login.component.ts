import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Router} from "@angular/router";
import {MojConfig} from "../MojConfig";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  username ="";
  password="";
  moguce_slanje = true;
  constructor(private http : HttpClient, private router : Router) { }

  ngOnInit(): void {
  }
  async prijava() {
    let obj = {
      username:this.username,
      password:this.password
    };
    this.moguce_slanje=false;
    let res =await this.http.post(MojConfig.adresa_servera+"/Authentication/prijava", obj).toPromise();
    // @ts-ignore
    if (res.prijavljen) {
      this.moguce_slanje=true;
      localStorage.setItem("my-token", JSON.stringify(res));
      this.router.navigate(["/studenti"]);
      return;
    }
    alert("Pogresni podaci!");
    this.password ="";
    this.moguce_slanje=true;
  }
}
