import { Component, OnInit } from '@angular/core';
import {MojConfig} from "../MojConfig";
import {HttpClient} from "@angular/common/http";
import {MyAuthService} from "../Services/MyAuthService";

@Component({
  selector: 'app-ocjene',
  templateUrl: './ocjene.component.html',
  styleUrls: ['./ocjene.component.css']
})
export class OcjeneComponent implements OnInit {
  studenti:any;
  studenti_copy:any;
  imeprezime ="";
  constructor(private http:HttpClient, public auth:MyAuthService) { }

  async ngOnInit() {
    await this.ucitajStudente();
  }
  async ucitajStudente() {
    this.studenti = await this.http.get(MojConfig.adresa_servera + "/Student/getStudenti")
      .toPromise();
    this.studenti_copy = this.studenti;
  }
  pretraga() {
    this.studenti_copy = this.studenti.filter((s:any)=> (s.ime + " " + s.prezime).toLowerCase().toLowerCase()
      .includes(this.imeprezime.toLowerCase()));
  }

}
