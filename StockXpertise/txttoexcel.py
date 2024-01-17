#coding: utf-8
import pandas as pd
from pathlib import Path
import sys

# R�cup�re les arguments pass�s au script
args = sys.argv

if len(args) > 1:
    input_path = args[1]

def txt_to_excel(input_path, output_path):
    # Lecture du fichier texte avec Pandas
    dataframe = pd.read_csv(input_path, encoding='latin-1')

    # Export du DataFrame vers un fichier Excel
    dataframe.to_excel(output_path, index=False)  # index=False pour ne pas inclure l'index dans le fichier Excel
    
    print(f"'{input_path}'  '{output_path}'")

# Utilisation de la fonction pour convertir le fichier texte en Excel
txt_file = 'lecturexel.txt'
p=Path(txt_file)

if p.exists():
    print(input_path)
    txt_to_excel(txt_file, input_path)