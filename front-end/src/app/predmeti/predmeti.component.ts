import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../MojConfig";

@Component({
  selector: 'app-predmeti',
  templateUrl: './predmeti.component.html',
  styleUrls: ['./predmeti.component.css']
})
export class PredmetiComponent implements OnInit {
  naziv="";
  dialog_novi_predmet = false;
  predmeti:any;
  predmeti_copy:any;
  profesori:any;
  ime ="";
  ects ="";
  semestar ="";
  nastavnikId =0;
  constructor(private http:HttpClient) { }

  async ngOnInit(){
    await this.preuzmiPredmete();
    await this.getProfesori();
  }
  pretraga() {
     this.predmeti_copy = this.predmeti.
     filter((p:any)=> p.naziv.toLowerCase().includes(this.naziv.toLowerCase()));
  }
  async preuzmiPredmete() {
    this.predmeti = await this.http.get(MojConfig.adresa_servera+"/Predmeti/getPredmeti").toPromise();
    this.predmeti_copy = this.predmeti;
  }
  async getProfesori() {
      this.profesori = await this.http.get(MojConfig.adresa_servera + "/Nastavnik/getNastavnici").toPromise();
  }
  async dodajPredmet() {
    let obj = {
      naziv: this.ime,
      ects : this.ects,
      semestar: this.semestar,
      nastavnikId: this.nastavnikId
    };
    let res = await this.http.post(MojConfig.adresa_servera+"/Predmeti/addPredmet", obj).toPromise();
    if(res) {
      alert("Uspjesno dodan predmet!");
      await this.preuzmiPredmete();
      await this.pretraga();
      this.obrisiPodatke();
      this.dialog_novi_predmet = !this.dialog_novi_predmet;
      return;
    }
      alert("Neuspjesno dodavanje predmeta!");
  }
  obrisiPodatke() {
  this.ime ="";
  this.ects ="";
  this.semestar ="";
  this.nastavnikId =0;
  }
  async obrisiPredmet(id:number) {
    let res = await this.http.delete
    (MojConfig.adresa_servera + "/Predmeti/deletePredmet?id="+id).toPromise();
    if(res) {
      await this.preuzmiPredmete();
      await this.pretraga();
      alert("Uspjesno brisanje predmeta!");
      return;
    }
    alert("Neuspjesno brisanje predmeta!");
  }
}
