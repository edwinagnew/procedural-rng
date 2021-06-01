from qiskit import IBMQ
from zipfile import ZipFile
IBMQ.enable_account('4757c06cbe81d7337300cb2c6309471a2bede57345c9b06d969caa0b0eb503c24a2352949086159eae6027ee6b93e4f561edbbc75548f6902594a6af8628b9e1')
provider = IBMQ.get_provider('ibm-q')
backend = provider.get_backend('ibmqx2')
job = provider.backend.retrieve_job('60b6a02a42c6a9deeef0d950')
job.wait_for_final_state()
results = job.result().get_memory()
file = open('/Users/edwinagnew/Desktop/RandomQC/procedural-rng/QRNG-unity/Assets/Bits/60b6a02a42c6a9deeef0d950.txt', 'w')
for r in results: file.write(r + ',')
file.close()
zip_name = file.name.rstrip('.txt') + '.zip'
with ZipFile(zip_name,'w') as zip: zip.write(file.name)
f = open('/Users/edwinagnew/Desktop/RandomQC/procedural-rng/QRNG-unity/Assets/Bits/rng_data.txt', 'a')
for r in results: f.write(r)
f.close()
print('done', job.status())
print('saved to', zip_name, 'and /Users/edwinagnew/Desktop/RandomQC/procedural-rng/QRNG-unity/Assets/Bits/rng_data.txt')