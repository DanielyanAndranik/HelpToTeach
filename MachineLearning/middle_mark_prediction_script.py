import requests
from sys import argv
import numpy as np

from sklearn.linear_model import LogisticRegression
from sklearn.externals import joblib

script, mode, url = argv

feature_names = ['studentId', 'hasScholarship', 'labsCount', 'labAbsenceCount', 'labMarkCount',
                 'labMark', 'lecturesCount', 'lectureAbsenceCount', 'lectureMarkCount',
                 'lectureMark', 'seminarsCount', 'seminarAbsenceCount', 'seminarMarkCount', 'seminarMark']
target_name = 'mark'
file_name = 'middle_mark_prediction_model.sav'

r = requests.get(url)
json_data = r.json()
X = []

for json_object in json_data:
    row = []
    for feature_name in feature_names:
        row.append(json_object[feature_name])
    X.append(row)

X = np.array(X)

Y = []
if mode == 'train':
    for json_object in json_data:
        Y.append(json_object[target_name])

    Y = np.array(Y)
    model = LogisticRegression()
    model.fit(X[:, 1:], Y)
    joblib.dump(model, file_name)

if mode == 'predict':
    model = joblib.load(file_name)
    Y = []
    for x in X:
        Y.append(model.predict(x))


