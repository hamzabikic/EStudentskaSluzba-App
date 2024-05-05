import { Component } from '@angular/core';
import {MyAuthService} from "./Services/MyAuthService";
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "./MojConfig";
import {Router} from "@angular/router";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'front-end';
  otvorenProfil = false;
  constructor(public myAuth : MyAuthService, private http : HttpClient, private router:Router) {
  }
  async odjava () {
    let res = await this.http.delete(MojConfig.adresa_servera+"/Authentication/odjava").toPromise();
    if(res) {
      localStorage.removeItem("my-token");
      await this.router.navigate(["/login"]);
      this.otvorenProfil= false;
      return;
    }
    alert("Neuspjesna odjava");
  }
}
