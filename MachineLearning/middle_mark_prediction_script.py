import requests
from sys import argv
import numpy as np

from sklearn.linear_model import LogisticRegression
from sklearn.externals import joblib

script, mode, get_url, post_url = argv

feature_names = ['studentId', 'hasScholarship', 'labsCount', 'labAbsenceCount', 'labMarkCount',
                 'labMark', 'lecturesCount', 'lectureAbsenceCount', 'lectureMarkCount',
                 'lectureMark', 'seminarsCount', 'seminarAbsenceCount', 'seminarMarkCount', 'seminarMark']
target_name = 'mark'
file_name = 'middle_mark_prediction_model.sav'

r = requests.get(get_url)
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
    result = []
    for x in X:
        result.append({x[0], model.predict(x)})

    r = requests.post(post_url, data=result)

