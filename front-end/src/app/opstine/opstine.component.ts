import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../MojConfig";

@Component({
  selector: 'app-opstine',
  templateUrl: './opstine.component.html',
  styleUrls: ['./opstine.component.css']
})
export class OpstineComponent implements OnInit {

  naziv="";
  dialog_nova_opstina = false;
  opstine:any;
  opstine_copy:any;
  drzave:any;
  ime ="";
  drzavaId =0;
  constructor(private http:HttpClient) { }

  async ngOnInit(){
    await this.getOpstine();
    await this.getDrzave();
  }
  pretraga() {
    this.opstine_copy = this.opstine.
    filter((p:any)=> p.naziv.toLowerCase().includes(this.naziv.toLowerCase()));
  }
  async getOpstine() {
    this.opstine = await this.http.get(MojConfig.adresa_servera+"/Opstina/getOpstine").toPromise();
    this.opstine_copy = this.opstine;
  }
  async getDrzave() {
    this.drzave= await this.http.get(MojConfig.adresa_servera + "/Opstina/getDrzave").toPromise();
  }
  async dodajOpstinu() {
    let obj = {
      naziv: this.ime,
      drzavaId: this.drzavaId
    };
    let res = await this.http.post(MojConfig.adresa_servera+"/Opstina/addOpstina", obj).toPromise();
    if(res) {
      alert("Uspjesno dodana opstina!");
      await this.getOpstine();
      await this.pretraga();
      this.obrisiPodatke();
      this.dialog_nova_opstina = !this.dialog_nova_opstina;
      return;
    }
    alert("Neuspjesno dodavanje opstine!");
  }
  obrisiPodatke() {
    this.ime ="";
    this.drzavaId =0;
  }
  async obrisiOpstinu(id:number) {
    let res = await this.http.delete
    (MojConfig.adresa_servera + "/Opstina/deleteOpstina?opstinaid="+id).toPromise();
    if(res) {
      await this.getOpstine();
      await this.pretraga();
      alert("Uspjesno brisanje opstine!");
      return;
    }
    alert("Neuspjesno brisanje opstine!");
  }
}
