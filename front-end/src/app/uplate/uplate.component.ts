import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../MojConfig";

@Component({
  selector: 'app-uplate',
  templateUrl: './uplate.component.html',
  styleUrls: ['./uplate.component.css']
})
export class UplateComponent implements OnInit {

  studenti:any;
  studenti_copy:any;
  imeprezime ="";
  otvori = false;
  constructor(private http:HttpClient) { }

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
